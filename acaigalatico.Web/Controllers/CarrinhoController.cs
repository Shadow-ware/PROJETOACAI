using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;
using acaigalatico.Infrastructure.Context;
using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    public class CarrinhoController : Controller
    {
        public class CartItem
        {
            public string Nome { get; set; }
            public string ImagemUrl { get; set; }
            public decimal Preco { get; set; }
            public int Quantidade { get; set; }
        }

        // Internal DTO stored in session representing a built pedido
        private class CartPedidoItem
        {
            public string Type { get; set; } = "Tradicional";
            public string Size { get; set; } = "300ml";
            public int Quantity { get; set; } = 1;
            public List<string>? Fruits { get; set; }
            public List<string>? Toppings { get; set; }
            public decimal UnitPrice { get; set; }
        }

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPagamentoService _pagamentoService;

        public CarrinhoController(AppDbContext context, UserManager<IdentityUser> userManager, IPagamentoService pagamentoService)
        {
            _context = context;
            _userManager = userManager;
            _pagamentoService = pagamentoService;
        }

        private const string SessionKey = "CartItems";

        private List<CartPedidoItem> GetCartFromSession()
        {
            var raw = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrWhiteSpace(raw)) return new List<CartPedidoItem>();
            try
            {
                return JsonSerializer.Deserialize<List<CartPedidoItem>>(raw) ?? new List<CartPedidoItem>();
            }
            catch
            {
                return new List<CartPedidoItem>();
            }
        }

        private void SaveCartToSession(List<CartPedidoItem> cart)
        {
            var raw = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(SessionKey, raw);
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Build view model from session cart
            var cart = GetCartFromSession();

            var items = cart.Select(c => new CartItem
            {
                Nome = $"Açaí {c.Type} {c.Size}",
                ImagemUrl = "/images/acai-expo-1.jpeg",
                Preco = c.UnitPrice, // unit price, view multiplies by quantity
                Quantidade = c.Quantity
            }).ToList();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([FromForm] string Type, [FromForm] string Size, [FromForm] int Quantity = 1)
        {
            // read fruits and toppings manually from Form
            var fruits = Request.Form.Where(kvp => kvp.Key == "Fruits").SelectMany(kvp => kvp.Value).ToList();
            var toppings = Request.Form.Where(kvp => kvp.Key == "Toppings").SelectMany(kvp => kvp.Value).ToList();

            // Simplified price calculation: use fixed mapping (should reuse OrderController.GetPrice in future)
            decimal unit = 10m;
            if (Type?.Contains("Gourmet", StringComparison.OrdinalIgnoreCase) == true) unit = 12m;
            if (Type?.Contains("Trufado", StringComparison.OrdinalIgnoreCase) == true) unit = 14m;
            if (Size?.Contains("400") == true) unit += 2;
            if (Size?.Contains("500") == true) unit += 5;
            if (Size?.Contains("700") == true) unit += 8;

            var cart = GetCartFromSession();
            cart.Add(new CartPedidoItem
            {
                Type = Type ?? "Tradicional",
                Size = Size ?? "300ml",
                Quantity = Quantity > 0 ? Quantity : 1,
                Fruits = fruits,
                Toppings = toppings,
                UnitPrice = unit
            });

            SaveCartToSession(cart);

            Console.WriteLine($"[CART] Item adicionado: {Type} {Size} x{Quantity} (unit {unit})");
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart([FromBody] RemoveCartRequest request)
        {
            if (request == null) return BadRequest("Requisição inválida");

            var cart = GetCartFromSession();
            if (request.Index < 0 || request.Index >= cart.Count) return BadRequest("Índice inválido");

            cart.RemoveAt(request.Index);
            SaveCartToSession(cart);

            return Ok();
        }

        public class RemoveCartRequest { public int Index { get; set; } }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromSession();
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Carrinho vazio.";
                return RedirectToAction("Index");
            }

            var (cliente, usuario) = await ResolveClienteAsync();
            var valorTotal = cart.Sum(c => c.UnitPrice * c.Quantity);

            // Payment processing based on posted fields
            var paymentMethod = Request.Form["PaymentMethod"].FirstOrDefault() ?? "card";
            string infoPagamento = "";

            if (paymentMethod == "card")
            {
                var cardName = Request.Form["CardName"].FirstOrDefault() ?? "";
                var cardNumber = Request.Form["CardNumber"].FirstOrDefault() ?? "";
                var cardExpiry = Request.Form["CardExpiry"].FirstOrDefault() ?? "";
                var cardCvv = Request.Form["CardCvv"].FirstOrDefault() ?? "";

                var result = await _pagamentoService.ProcessarPagamentoCartaoAsync(cardName, cardNumber, cardExpiry, cardCvv, valorTotal);
                if (!result.Sucesso)
                {
                    TempData["ErrorMessage"] = result.Mensagem;
                    return RedirectToAction("Index", "Carrinho");
                }

                infoPagamento = $"Cartão (Transação: {result.TransacaoId})";
            }
            else if (paymentMethod == "pix")
            {
                var result = await _pagamentoService.GerarPixAsync(valorTotal);
                infoPagamento = "Pix (Aguardando Confirmação)";
            }

            // Build Venda and items
            var identityUser = await _userManager.GetUserAsync(User);
            var email = await _userManager.GetEmailAsync(identityUser) ?? identityUser?.Email ?? "Visitante";

            var pedido = new Venda
            {
                DataVenda = DateTime.Now,
                ValorTotal = valorTotal,
                FormaPagamento = paymentMethod == "pix" ? TipoPagamento.Pix : TipoPagamento.Cartao,
                ClienteId = cliente?.Id,
                Status = paymentMethod == "card" ? StatusVenda.Preparando : StatusVenda.Pendente,
                EnderecoEntrega = usuario?.Endereco ?? "Retirada/Uber Moto",
                BairroEntrega = "",
                Observacao = $"Cliente: {usuario?.Nome ?? "Visitante"} ({email}) | Pedido via Carrinho | Pagamento: {infoPagamento}",
                Itens = new List<ItemVenda>()
            };

            foreach (var c in cart)
            {
                pedido.Itens.Add(new ItemVenda { Quantidade = c.Quantity, PrecoUnitario = c.UnitPrice, ProdutoId = 1 });
            }

            // Try sync with API similar to OrderController
            bool enviadoParaApi = false;
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);

                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                string[] urls = { "http://127.0.0.1:5207/api/pedidos", "http://localhost:5207/api/pedidos" };
                foreach (var url in urls)
                {
                    try
                    {
                        var pedidoParaApi = new {
                            dataVenda = pedido.DataVenda,
                            valorTotal = pedido.ValorTotal,
                            formaPagamento = (int)pedido.FormaPagamento,
                            status = (int)pedido.Status,
                            enderecoEntrega = pedido.EnderecoEntrega,
                            bairroEntrega = pedido.BairroEntrega,
                            observacao = pedido.Observacao,
                            clienteId = pedido.ClienteId,
                            itens = (object?)null
                        };

                        var response = await client.PostAsJsonAsync(url, pedidoParaApi, jsonOptions);
                        if (response.IsSuccessStatusCode)
                        {
                            enviadoParaApi = true;
                            break;
                        }
                    }
                    catch { }
                }
            }
            catch { }

            // Save local
            _context.Vendas.Add(pedido);
            await _context.SaveChangesAsync();

            if (!enviadoParaApi)
            {
                TempData["WarningMessage"] = "Pedido salvo localmente, não sincronizado com a central.";
            }

            SaveCartToSession(new List<CartPedidoItem>());
            TempData["SuccessMessage"] = "Compra realizada com sucesso!";
            return RedirectToAction("Index", "Pedidos");
        }

        private async Task<(Cliente? Cliente, Usuario? Usuario)> ResolveClienteAsync()
        {
            if (User.Identity?.IsAuthenticated != true) return (null, null);
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null) return (null, null);

            var email = await _userManager.GetEmailAsync(identityUser) ?? identityUser.Email ?? string.Empty;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            var telefone = usuario?.Telefone;
            if (string.IsNullOrWhiteSpace(telefone)) telefone = identityUser.PhoneNumber;

            Cliente? cliente = null;
            if (!string.IsNullOrWhiteSpace(telefone))
            {
                cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Telefone == telefone);
            }

            if (cliente == null)
            {
                // Create a new cliente if missing (same behavior as OrderController.ResolveClienteAtualAsync)
                var nome = LimitText((usuario?.Nome ?? identityUser.UserName ?? email ?? "Cliente").Trim(), 100);
                var telefoneToSave = string.IsNullOrWhiteSpace(telefone) ? LimitText($"USR-{(identityUser.Id ?? "cliente").Replace("-", "")}", 20) : LimitText(telefone.Trim(), 20);

                cliente = new Cliente
                {
                    Nome = nome,
                    Telefone = telefoneToSave,
                    SaldoDevedor = 0
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }

            return (cliente, usuario);
        }

        private static string LimitText(string text, int limit)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= limit ? text : text.Substring(0, limit);
        }
    }
}
