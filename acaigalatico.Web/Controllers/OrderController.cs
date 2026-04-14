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
using acaigalatico.Application.Interfaces;
using acaigalatico.Web.ViewModels;

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
        private readonly IPagamentoService _pagamentoService;

        public OrderController(AppDbContext context, UserManager<IdentityUser> userManager, IPagamentoService pagamentoService)
        {
            _context = context;
            _userManager = userManager;
            _pagamentoService = pagamentoService;
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

            var (_, usuario) = await ResolveClienteAtualAsync();
            ViewBag.Usuario = usuario;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(PedidoViewModel model)
        {
            // Força valores padrão se vierem nulos ou vazios
            model.Type = NormalizeType(model.Type);
            model.Size = NormalizeSize(model.Size);
            if (model.Quantity <= 0) model.Quantity = 1;
            if (string.IsNullOrWhiteSpace(model.PaymentMethod)) model.PaymentMethod = "card";

            // Debug dos valores recebidos
            Console.WriteLine($"[DEBUG-SUBMIT] Recebido: Tipo='{model.Type}', Tamanho='{model.Size}', Quantidade={model.Quantity}, Metodo='{model.PaymentMethod}'");

            // Se o método de pagamento não for cartão, removemos a validação dos campos de cartão
            if (model.PaymentMethod != "card")
            {
                ModelState.Remove(nameof(model.CardName));
                ModelState.Remove(nameof(model.CardNumber));
                ModelState.Remove(nameof(model.CardExpiry));
                ModelState.Remove(nameof(model.CardCvv));
            }

            if (!ModelState.IsValid)
            {
                // Log dos erros para facilitar o diagnóstico no console
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"[VALIDATION-ERROR] Campo '{state.Key}': {error.ErrorMessage}");
                    }
                }

                TempData["ErrorMessage"] = "Por favor, preencha todos os campos obrigatórios corretamente.";
                return RedirectToAction(nameof(Index));
            }

            var contents = await GetSiteContentsAsync();
            var tipoSelecionado = NormalizeType(model.Type);
            var tamanhoSelecionado = NormalizeSize(model.Size);
            var quantidade = model.Quantity > 0 ? model.Quantity : 1;

            var frutasSelecionadas = (model.Fruits ?? new List<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .Take(GetLimit(contents, tamanhoSelecionado, true))
                .ToList();

            var acompanhamentosSelecionados = (model.Toppings ?? new List<string>())
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

            var valorTotal = valorUnitario * quantidade;

            // --- PROCESSAMENTO DE PAGAMENTO ---
            string infoPagamento = "";
            if (model.PaymentMethod == "card")
            {
                var result = await _pagamentoService.ProcessarPagamentoCartaoAsync(
                    model.CardName ?? "", model.CardNumber ?? "", model.CardExpiry ?? "", model.CardCvv ?? "", valorTotal);

                if (!result.Sucesso)
                {
                    TempData["ErrorMessage"] = result.Mensagem;
                    return RedirectToAction(nameof(Index));
                }
                infoPagamento = $"Cartão (Transação: {result.TransacaoId})";
            }
            else if (model.PaymentMethod == "pix")
            {
                var result = await _pagamentoService.GerarPixAsync(valorTotal);
                infoPagamento = "Pix (Aguardando Confirmação)";
            }

            var (cliente, usuario) = await ResolveClienteAtualAsync();
            var email = usuario?.Email ?? (await _userManager.GetUserAsync(User))?.Email ?? "Visitante";
            
            // Log para debug no console do servidor
            if (cliente == null) 
                Console.WriteLine("[DEBUG] Cliente não resolvido para o pedido.");
            else 
                Console.WriteLine($"[DEBUG] Pedido vinculado ao Cliente ID: {cliente.Id}");

            var enderecoEntrega = string.IsNullOrWhiteSpace(usuario?.Endereco) ? "Retirada/Uber Moto" : usuario.Endereco.Trim();
            var bairroEntrega = ExtractNeighborhood(enderecoEntrega);

            var pedido = new Venda
            {
                DataVenda = DateTime.Now,
                ValorTotal = valorTotal,
                FormaPagamento = MapPaymentMethod(model.PaymentMethod),
                ClienteId = cliente?.Id,
                Status = model.PaymentMethod == "card" ? StatusVenda.Preparando : StatusVenda.Pendente,
                EnderecoEntrega = LimitText(enderecoEntrega, 200),
                BairroEntrega = LimitText(bairroEntrega, 100),
                Observacao = $"Cliente: {usuario?.Nome ?? "Visitante"} ({email}) | " + BuildObservation(tipoSelecionado, tamanhoSelecionado, quantidade, frutasSelecionadas, acompanhamentosSelecionados, model.PaymentMethod) + $" | Pagamento: {infoPagamento}",
                Itens = new List<ItemVenda>()
            };

            // Criar um item representativo do pedido de açaí
            pedido.Itens.Add(new ItemVenda
            {
                Quantidade = quantidade,
                PrecoUnitario = valorUnitario,
                // Como não temos um ProdutoId fixo para cada tipo de açaí no banco, 
                // vamos tentar encontrar um ou deixar 0 (o banco pode reclamar se houver FK)
                // Mas para a integração funcionar, o importante é o envio.
                ProdutoId = 1 // Placeholder ou buscar um produto real se existir
            });

            int pedidoId = 0;

            // --- SINCRONIZAÇÃO OBRIGATÓRIA COM A API ---
            bool enviadoParaApi = false;
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(15);
                
                // Configura as opções de serialização para garantir compatibilidade com a API (camelCase)
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                // Tenta enviar tanto para 127.0.0.1 quanto para localhost para evitar problemas de resolução DNS/IPv6
                string[] urls = { "http://127.0.0.1:5207/api/pedidos", "http://localhost:5207/api/pedidos" };
                
                foreach (var url in urls)
                {
                    try 
                    {
                        Console.WriteLine($"[API-SYNC] Tentando sincronizar com: {url}");
                        
                        // Garante que o pedido está limpo para a API e simplificado para evitar erros de FK
                        var pedidoParaApi = new {
                            dataVenda = pedido.DataVenda,
                            valorTotal = pedido.ValorTotal,
                            formaPagamento = (int)pedido.FormaPagamento,
                            status = (int)pedido.Status,
                            enderecoEntrega = pedido.EnderecoEntrega,
                            bairroEntrega = pedido.BairroEntrega,
                            observacao = pedido.Observacao,
                            clienteId = pedido.ClienteId, // Mantém o vínculo se existir
                            itens = (object?)null
                        };

                        var response = await client.PostAsJsonAsync(url, pedidoParaApi, jsonOptions);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            enviadoParaApi = true;
                            var responseData = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"[API-SYNC] Pedido sincronizado com sucesso via {url}! Resposta: {responseData}");
                            TempData["SuccessMessage"] = "Pedido realizado e sincronizado com a central!";
                            break; 
                        }
                        else
                        {
                            var erro = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"[API-SYNC-WARN] API em {url} recusou o pedido ({response.StatusCode}): {erro}");
                            TempData["ApiError"] = $"API recusou ({response.StatusCode}): {erro}";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[API-SYNC-ERROR] Falha ao conectar com {url}: {ex.Message}");
                        TempData["ApiError"] = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API-ERROR] Falha crítica ao conectar com API: {ex.Message}");
            }

            // Se não conseguiu enviar para a API, já avisamos no console mas salvamos LOCALMENTE de qualquer forma
            // para que o usuário possa ver seu pedido na página "Meus Pedidos" que consome o banco local.
            try
            {
                _context.Vendas.Add(pedido);
                await _context.SaveChangesAsync();
                pedidoId = pedido.Id;
                Console.WriteLine($"[DB-LOCAL] Pedido #{pedidoId} salvo localmente no banco do Web.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB-LOCAL-ERROR] Falha ao salvar localmente: {ex.Message}");
            }

            if (!enviadoParaApi)
            {
                var lastError = TempData["ApiError"]?.ToString() ?? "Conexão recusada pela API.";
                TempData["WarningMessage"] = $"Seu pedido foi registrado localmente, mas não pôde ser sincronizado com a central: {lastError}";
            }

            if (!TempData.ContainsKey("SuccessMessage"))
            {
                TempData["SuccessMessage"] = "Pedido realizado com sucesso! Em breve entregaremos sua galáxia de sabor.";
            }
            
            return RedirectToAction("Index", "Pedidos");
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
            // Log para debug
            Console.WriteLine($"[DEBUG-PRICE] Calculando preço para Tipo: '{type}' e Tamanho: '{size}'");

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

            if (contents.TryGetValue(key, out var rawValue))
            {
                var price = ParseDecimal(rawValue);
                Console.WriteLine($"[DEBUG-PRICE] Chave: {key}, Valor Bruto: {rawValue}, Preço Final: {price}");
                if (price > 0) return price;
            }

            Console.WriteLine($"[DEBUG-PRICE] Chave '{key}' não encontrada ou valor inválido. Usando fallback dinâmico.");
            
            // Fallback para valores padrão se o banco estiver vazio
            // Tradicional: 10/12/15/18
            // Gourmet: 12/15/18/22
            // Trufado: 14/18/22/28
            return type switch
            {
                "Gourmet" => size switch {
                    "300ml" => 12.00m,
                    "400ml" => 15.00m,
                    "500ml" => 18.00m,
                    "700ml" => 22.00m,
                    _ => 12.00m
                },
                "Trufado" => size switch {
                    "300ml" => 14.00m,
                    "400ml" => 18.00m,
                    "500ml" => 22.00m,
                    "700ml" => 28.00m,
                    _ => 14.00m
                },
                _ => size switch {
                    "300ml" => 10.00m,
                    "400ml" => 12.00m,
                    "500ml" => 15.00m,
                    "700ml" => 18.00m,
                    _ => 10.00m
                }
            };
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
            if (string.IsNullOrWhiteSpace(type)) return "Tradicional";
            
            if (type.Contains("Trufado", StringComparison.OrdinalIgnoreCase)) return "Trufado";
            if (type.Contains("Gourmet", StringComparison.OrdinalIgnoreCase)) return "Gourmet";
            
            return "Tradicional";
        }

        private static string NormalizeSize(string? size)
        {
            if (string.IsNullOrWhiteSpace(size)) return "300ml";

            if (size.Contains("400")) return "400ml";
            if (size.Contains("500")) return "500ml";
            if (size.Contains("700")) return "700ml";
            
            return "300ml";
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
