using acaigalatico.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace acaigalatico.Web.Controllers
{
    public class ToppingOption
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    // [Authorize] removed to allow guests to order
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var contents = await GetSiteContentsAsync();

            if (contents.TryGetValue("Order_Side_Image", out var sideImg))
            {
                if (string.IsNullOrWhiteSpace(sideImg) || sideImg.Contains("logo", StringComparison.OrdinalIgnoreCase))
                {
                    contents["Order_Side_Image"] = "/images/acai-expo-1.jpeg";
                }
            }
            
            ViewBag.SiteContents = contents;

            ViewBag.Fruits = BuildFruits(contents);
            ViewBag.Toppings = BuildToppings(contents);

            return View();
        }

        private static List<string> BuildFruits(Dictionary<string, string> contents)
        {
            if (contents != null && contents.TryGetValue("Order_Fruits", out var raw) && !string.IsNullOrWhiteSpace(raw))
            {
                var items = raw
                    .Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList();

                if (items.Count > 0) return items;
            }

            return new List<string> { "Banana", "Morango", "Manga", "Kiwi", "Abacaxi", "Uva" };
        }

        private static List<ToppingOption> BuildToppings(Dictionary<string, string> contents)
        {
            if (contents != null && contents.TryGetValue("Order_Toppings", out var raw) && !string.IsNullOrWhiteSpace(raw))
            {
                var lines = raw.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var parsed = new List<ToppingOption>();

                foreach (var line in lines)
                {
                    var parts = line.Split('|').Select(p => p.Trim()).ToArray();
                    if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0])) continue;

                    parsed.Add(new ToppingOption
                    {
                        Name = parts[0],
                        Description = parts.Length > 1 ? parts[1] : "",
                        ImageUrl = parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]) ? parts[2] : "/images/logo-sem-escrita.png"
                    });
                }

                if (parsed.Count > 0) return parsed;
            }

            return new List<ToppingOption>
            {
                new ToppingOption { Name = "Creme de Paçoca", Description = "Uma camada deliciosa de creme de amendoim.", ImageUrl = "/images/logo-sem-escrita.png" },
                new ToppingOption { Name = "Creme de Pitaya", Description = "Sabor exótico e cor vibrante.", ImageUrl = "/images/logo-sem-escrita.png" },
                new ToppingOption { Name = "Creme de Leite em Pó", Description = "Cremosidade extra para seu açaí.", ImageUrl = "/images/logo-sem-escrita.png" },
                new ToppingOption { Name = "Leite em Pó", Description = "Clássico indispensável.", ImageUrl = "/images/logo-sem-escrita.png" },
                new ToppingOption { Name = "Granola", Description = "Crocância saudável.", ImageUrl = "/images/logo-sem-escrita.png" },
                new ToppingOption { Name = "Paçoca", Description = "O sabor do amendoim.", ImageUrl = "/images/logo-sem-escrita.png" }
            };
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder(string type, string size, int quantity, List<string>? fruits, List<string>? toppings, string? paymentMethod)
        {
            var contents = await GetSiteContentsAsync();
            var tipoSelecionado = NormalizeType(type);
            var tamanhoSelecionado = NormalizeSize(size);
            var quantidade = quantity < 1 ? 1 : quantity;

            var frutasSelecionadas = (fruits ?? new List<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .Take(GetLimit(contents, tamanhoSelecionado, true))
                .ToList();

            var acompanhamentosSelecionados = (toppings ?? new List<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .Take(GetLimit(contents, tamanhoSelecionado, false))
                .ToList();

            var valorUnitario = GetPrice(contents, tipoSelecionado, tamanhoSelecionado);
            if (valorUnitario <= 0)
            {
                TempData["ErrorMessage"] = "Não foi possível calcular o valor do pedido.";
                return RedirectToAction(nameof(Index));
            }

            var (cliente, usuario) = await ResolveClienteAtualAsync();
            var enderecoEntrega = string.IsNullOrWhiteSpace(usuario?.Endereco) ? "Retirada/Uber Moto" : usuario.Endereco.Trim();
            var bairroEntrega = ExtractNeighborhood(enderecoEntrega);

            var pedido = new Venda
            {
                DataVenda = DateTime.Now,
                ValorTotal = valorUnitario * quantidade,
                FormaPagamento = MapPaymentMethod(paymentMethod),
                ClienteId = cliente?.Id,
                Status = StatusVenda.Pendente,
                EnderecoEntrega = LimitText(enderecoEntrega, 200),
                BairroEntrega = LimitText(bairroEntrega, 100),
                Observacao = BuildObservation(tipoSelecionado, tamanhoSelecionado, quantidade, frutasSelecionadas, acompanhamentosSelecionados, paymentMethod)
            };

            _context.Vendas.Add(pedido);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Pedido realizado com sucesso! Em breve entregaremos sua galáxia de sabor.";
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Pedidos");
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<Dictionary<string, string>> GetSiteContentsAsync()
        {
            try
            {
                return await _context.SiteContents
                    .AsNoTracking()
                    .Where(c => c.Page == "FazerPedido" || c.Page == "Cardapio")
                    .ToDictionaryAsync(c => c.Key, c => c.Value);
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        private async Task<(Cliente? Cliente, Usuario? Usuario)> ResolveClienteAtualAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return (null, null);
            }

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return (null, null);
            }

            var email = await _userManager.GetEmailAsync(identityUser) ?? identityUser.Email ?? string.Empty;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var nome = LimitText((usuario?.Nome ?? identityUser.UserName ?? email ?? "Cliente").Trim(), 100);
            var telefone = BuildClientIdentifier(identityUser, usuario);

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Telefone == telefone);
            if (cliente == null)
            {
                cliente = new Cliente
                {
                    Nome = nome,
                    Telefone = telefone,
                    SaldoDevedor = 0
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                var atualizado = false;

                if (cliente.Nome != nome)
                {
                    cliente.Nome = nome;
                    atualizado = true;
                }

                if (cliente.Telefone != telefone)
                {
                    cliente.Telefone = telefone;
                    atualizado = true;
                }

                if (atualizado)
                {
                    _context.Clientes.Update(cliente);
                    await _context.SaveChangesAsync();
                }
            }

            return (cliente, usuario);
        }

        private static string BuildClientIdentifier(IdentityUser identityUser, Usuario? usuario)
        {
            var telefone = usuario?.Telefone;
            if (string.IsNullOrWhiteSpace(telefone))
            {
                telefone = identityUser.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(telefone))
            {
                return LimitText(telefone.Trim(), 20);
            }

            if (usuario != null)
            {
                return LimitText($"USR-{usuario.Id}", 20);
            }

            var fallback = (identityUser.Id ?? "cliente").Replace("-", "");
            return LimitText($"USR-{fallback}", 20);
        }

        private static decimal GetPrice(Dictionary<string, string> contents, string type, string size)
        {
            var key = type switch
            {
                "Trufado" => size switch
                {
                    "300ml" => "Menu_Trufado_Price_300ml",
                    "400ml" => "Menu_Trufado_Price_400ml",
                    "500ml" => "Menu_Trufado_Price_500ml",
                    "700ml" => "Menu_Trufado_Price_700ml",
                    _ => "Menu_Trufado_Price_300ml"
                },
                "Gourmet" => size switch
                {
                    "300ml" => "Menu_Gourmet_Price_300ml",
                    "400ml" => "Menu_Gourmet_Price_400ml",
                    "500ml" => "Menu_Gourmet_Price_500ml",
                    "700ml" => "Menu_Gourmet_Price_700ml",
                    _ => "Menu_Gourmet_Price_300ml"
                },
                _ => size switch
                {
                    "300ml" => "Menu_Price_300ml",
                    "400ml" => "Menu_Price_400ml",
                    "500ml" => "Menu_Price_500ml",
                    "700ml" => "Menu_Price_700ml",
                    _ => "Menu_Price_300ml"
                }
            };

            return contents.TryGetValue(key, out var rawValue)
                ? ParseDecimal(rawValue)
                : 0m;
        }

        private static int GetLimit(Dictionary<string, string> contents, string size, bool fruits)
        {
            var key = size switch
            {
                "400ml" => fruits ? "Menu_Limit_400ml_Fruits" : "Menu_Limit_400ml_Toppings",
                "500ml" => fruits ? "Menu_Limit_500ml_Fruits" : "Menu_Limit_500ml_Toppings",
                "700ml" => fruits ? "Menu_Limit_700ml_Fruits" : "Menu_Limit_700ml_Toppings",
                _ => fruits ? "Menu_Limit_300ml_Fruits" : "Menu_Limit_300ml_Toppings"
            };

            return contents.TryGetValue(key, out var rawValue) && int.TryParse(rawValue, out var limit) && limit > 0
                ? limit
                : (fruits ? 1 : 2);
        }

        private static TipoPagamento MapPaymentMethod(string? paymentMethod)
        {
            return string.Equals(paymentMethod, "pix", StringComparison.OrdinalIgnoreCase)
                ? TipoPagamento.Pix
                : TipoPagamento.Cartao;
        }

        private static string NormalizeType(string? type)
        {
            return type switch
            {
                "Trufado" => "Trufado",
                "Gourmet" => "Gourmet",
                _ => "Tradicional"
            };
        }

        private static string NormalizeSize(string? size)
        {
            return size switch
            {
                "400ml" => "400ml",
                "500ml" => "500ml",
                "700ml" => "700ml",
                _ => "300ml"
            };
        }

        private static decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0m;
            }

            return decimal.TryParse(value.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : 0m;
        }

        private static string BuildObservation(string type, string size, int quantity, List<string> fruits, List<string> toppings, string? paymentMethod)
        {
            var partes = new List<string>
            {
                $"Tipo: {type}",
                $"Tamanho: {size}",
                $"Quantidade: {quantity}",
                $"Frutas: {(fruits.Count > 0 ? string.Join(", ", fruits) : "Nenhuma")}",
                $"Acompanhamentos: {(toppings.Count > 0 ? string.Join(", ", toppings) : "Nenhum")}",
                $"Pagamento: {MapPaymentMethod(paymentMethod)}"
            };

            return LimitText(string.Join(" | ", partes), 500);
        }

        private static string ExtractNeighborhood(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
            {
                return "Não informado";
            }

            var parts = endereco.Split('-', 2, StringSplitOptions.TrimEntries);
            if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                return parts[1];
            }

            return "Não informado";
        }

        private static string LimitText(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var trimmed = value.Trim();
            return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
        }
    }
}
