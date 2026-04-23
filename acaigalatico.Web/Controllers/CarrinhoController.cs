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
        public async Task<IActionResult> Adicionar(acaigalatico.Web.ViewModels.PedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dados do pedido inválidos." });
            }

            try
            {
                // 1. Mapear para o formato interno do carrinho
                var newItem = new CartPedidoItem
                {
                    Type = model.Tipo ?? model.Type ?? "Tradicional",
                    Size = model.Tamanho ?? model.Size ?? "300ml",
                    Quantity = model.Quantidade > 0 ? model.Quantidade : (model.Quantity > 0 ? model.Quantity : 1),
                    Fruits = !string.IsNullOrEmpty(model.Frutas) ? model.Frutas.Split(',').Select(f => f.Trim()).ToList() : (model.Fruits ?? new List<string>()),
                    Toppings = !string.IsNullOrEmpty(model.Acompanhamentos) ? model.Acompanhamentos.Split(',').Select(t => t.Trim()).ToList() : (model.Toppings ?? new List<string>()),
                    UnitPrice = model.Valor > 0 ? model.Valor / (model.Quantidade > 0 ? model.Quantidade : 1) : 15.00m // Fallback de preço
                };

                // Se o preço unitário ainda for muito baixo, tentamos calcular
                if (newItem.UnitPrice <= 5) newItem.UnitPrice = 15.00m;

                // 2. Adicionar ao carrinho na sessão
                var cart = GetCartFromSession();
                cart.Add(newItem);
                SaveCartToSession(cart);

                Console.WriteLine($"[DEBUG] Item adicionado via Adicionar: {newItem.Type} {newItem.Size}");

                // 3. Retornar sucesso para o fetch
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro no Adicionar: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([FromForm] string Type, [FromForm] string Size, [FromForm] int Quantity = 1, [FromForm] List<string>? Fruits = null, [FromForm] List<string>? Toppings = null, [FromForm] decimal Valor = 0)
        {
            try
            {
                // Binding automático deve funcionar com FromForm e URLSearchParams
                var fruits = Fruits ?? new List<string>();
                var toppings = Toppings ?? new List<string>();

                // Preço unitário baseado no valor enviado ou cálculo manual
                decimal unit = Valor > 0 ? Valor / (Quantity > 0 ? Quantity : 1) : 0;
                
                if (unit <= 0)
                {
                    unit = 10m;
                    if (!string.IsNullOrEmpty(Type))
                    {
                        if (Type.Contains("Gourmet", StringComparison.OrdinalIgnoreCase)) unit = 15m;
                        else if (Type.Contains("Trufado", StringComparison.OrdinalIgnoreCase)) unit = 13m;
                    }

                    if (!string.IsNullOrEmpty(Size))
                    {
                        if (Size.Contains("400")) unit += 2;
                        else if (Size.Contains("500")) unit += 5;
                        else if (Size.Contains("700")) unit += 10;
                    }
                }

                var cart = GetCartFromSession();
                var newItem = new CartPedidoItem
                {
                    Type = Type ?? "Tradicional",
                    Size = Size ?? "300ml",
                    Quantity = Quantity > 0 ? Quantity : 1,
                    Fruits = fruits,
                    Toppings = toppings,
                    UnitPrice = unit
                };
                cart.Add(newItem);

                SaveCartToSession(cart);
                return Json(new { success = true, itemCount = cart.Count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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

            // CONSTRUÇÃO DAS OBSERVAÇÕES DETALHADAS (CRÍTICO PARA O DESKTOP)
            var observationParts = new List<string>();
            
            // Se houver apenas um item, usamos o formato simples que o Desktop entende bem
            if (cart.Count == 1)
            {
                var c = cart[0];
                observationParts.Add($"Tipo: {c.Type}");
                observationParts.Add($"Tamanho: {c.Size}");
                observationParts.Add($"Quantidade: {c.Quantity}");
                observationParts.Add($"Frutas: {(c.Fruits != null && c.Fruits.Any() ? string.Join(", ", c.Fruits) : "Nenhuma")}");
                observationParts.Add($"Acompanhamentos: {(c.Toppings != null && c.Toppings.Any() ? string.Join(", ", c.Toppings) : "Nenhum")}");
            }
            else
            {
                // Para múltiplos itens, tentamos um resumo ou listagem
                observationParts.Add($"Resumo: {cart.Count} itens no carrinho");
                for (int i = 0; i < cart.Count; i++)
                {
                    var c = cart[i];
                    observationParts.Add($"Item {i+1}: {c.Quantity}x {c.Type} {c.Size}");
                }
            }

            observationParts.Add($"Cliente: {usuario?.Nome ?? "Visitante"} ({email})");
            observationParts.Add($"Pagamento: {infoPagamento}");

            var pedido = new Venda
            {
                DataVenda = DateTime.Now,
                ValorTotal = valorTotal,
                FormaPagamento = paymentMethod == "pix" ? TipoPagamento.Pix : TipoPagamento.Cartao,
                ClienteId = cliente?.Id,
                Status = paymentMethod == "card" ? StatusVenda.Preparando : StatusVenda.Pendente,
                EnderecoEntrega = usuario?.Endereco ?? "Retirada/Uber Moto",
                BairroEntrega = "",
                Observacao = LimitText(string.Join(" | ", observationParts), 500),
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
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                // Enviamos um objeto simplificado para a API para garantir compatibilidade e evitar erros de FK/Navegação
                var pedidoParaApi = new
                {
                    dataVenda = pedido.DataVenda,
                    valorTotal = pedido.ValorTotal,
                    formaPagamento = (int)pedido.FormaPagamento,
                    status = (int)pedido.Status,
                    enderecoEntrega = pedido.EnderecoEntrega,
                    bairroEntrega = pedido.BairroEntrega,
                    observacao = pedido.Observacao,
                    clienteId = (int?)null, // Evita erro de FK se o ID do cliente não existir na API
                    itens = (object?)null  // O Desktop lê os detalhes da Observação
                };

                string[] urls = { "http://127.0.0.1:5207/api/pedidos", "http://localhost:5207/api/pedidos" };
                foreach (var url in urls)
                {
                    try
                    {
                        var response = await client.PostAsJsonAsync(url, pedidoParaApi, jsonOptions);
                        if (response.IsSuccessStatusCode)
                        {
                            enviadoParaApi = true;
                            Console.WriteLine($"[API-SYNC-SUCCESS] Pedido enviado para {url}");
                            break;
                        }
                        else
                        {
                            var errorBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"[API-SYNC-FAIL] {url} retornou {response.StatusCode}: {errorBody}");
                        }
                    }
                    catch (Exception apiEx)
                    {
                        Console.WriteLine($"[API-SYNC-ERROR] Falha ao enviar para {url}: {apiEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API-SYNC-CRITICAL] {ex.Message}");
            }

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
