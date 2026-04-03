using acaigalatico.Application.Interfaces;
using acaigalatico.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;

namespace acaigalatico.Web.Controllers
{
    [AllowAnonymous] // Temporário para facilitar a visualização do usuário
    public class AdminController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly IClienteService _clienteService;
        private readonly IVendaService _vendaService;
        private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _environment;

        public AdminController(
            IProdutoService produtoService,
            IClienteService clienteService,
            IVendaService vendaService,
            acaigalatico.Infrastructure.Context.AppDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<AdminController> logger,
            IWebHostEnvironment environment)
        {
            _produtoService = produtoService;
            _clienteService = clienteService;
            _vendaService = vendaService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SeedMockDataInterno(); // Auto-seed se estiver vazio
            
            var pedidos = await _vendaService.GetVendasAsync();
            var clientes = await _clienteService.GetClientesAsync();
            var produtos = await _produtoService.GetProdutosAsync();

            // Métricas Reais do Banco
            var realTotalVendas = pedidos.Count();
            var realFaturamento = pedidos.Where(v => v.Status == StatusVenda.Entregue).Sum(v => v.ValorTotal);
            var realTotalClientes = clientes.Count();

            // Tenta obter métricas manuais (se existirem)
            var m_vendasHoje = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_VendasHoje");
            var m_totalClientes = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_TotalClientes");
            var m_faturamento = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_Faturamento");

            ViewBag.VendasHoje = m_vendasHoje?.Value ?? realTotalVendas.ToString();
            ViewBag.TotalClientes = m_totalClientes?.Value ?? realTotalClientes.ToString();
            
            if (m_faturamento != null && decimal.TryParse(m_faturamento.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal f))
            {
                ViewBag.Faturamento = f;
            }
            else
            {
                ViewBag.Faturamento = realFaturamento;
            }

            ViewBag.TotalProdutos = produtos.Count();

            // Dados para os gráficos (Mockados para visualização)
            ViewBag.SalesLabels = new List<string> { "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom" };
            ViewBag.SalesData = new List<decimal> { 120.50m, 190.00m, 300.00m, 500.00m, 230.00m, 340.00m, 450.00m };

            return View();
        }

        // ================================
        // 🔥 PEDIDOS (GESTÃO DE VENDAS)
        // ================================
        [HttpGet]
        public async Task<IActionResult> Pedidos()
        {
            _logger.LogInformation("Acessando Pedidos Admin");
            var pedidos = await _vendaService.GetVendasAsync();
            ViewBag.Clientes = await _clienteService.GetClientesAsync();
            return View(pedidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePedido(string valorTotal, StatusVenda status, string endereco, string bairro, string observacao, int? clienteId)
        {
            _logger.LogInformation($"Criando Pedido: Valor={valorTotal}, Status={status}, ClienteId={clienteId}");
            try
            {
                decimal valor = 0;
                if (!string.IsNullOrEmpty(valorTotal))
                {
                    string valorStr = valorTotal.Replace(",", ".");
                    if (!decimal.TryParse(valorStr, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                    {
                        TempData["ErrorMessage"] = "Valor total inválido.";
                        return RedirectToAction("Pedidos");
                    }
                }

                var novoPedido = new Venda
                {
                    ValorTotal = valor,
                    Status = status,
                    EnderecoEntrega = endereco ?? "",
                    BairroEntrega = bairro ?? "",
                    Observacao = observacao ?? "",
                    ClienteId = clienteId,
                    DataVenda = DateTime.Now,
                    FormaPagamento = TipoPagamento.Dinheiro
                };

                _context.Vendas.Add(novoPedido);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Pedido {novoPedido.Id} criado com sucesso no banco.");
                TempData["SuccessMessage"] = "Novo pedido criado com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido");
                TempData["ErrorMessage"] = "Erro ao criar pedido: " + ex.Message;
            }
            return RedirectToAction("Pedidos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPedido(int id, string valorTotal, StatusVenda status, string enderecoEntrega, string bairroEntrega, string observacao, int? clienteId)
        {
            _logger.LogInformation($"Editando Pedido {id}: Valor={valorTotal}, Status={status}");
            try 
            {
                var pedido = await _context.Vendas.FindAsync(id);
                if (pedido != null)
                {
                    decimal valor = 0;
                    if (!string.IsNullOrEmpty(valorTotal))
                    {
                        string valorStr = valorTotal.Replace(",", ".");
                        if (!decimal.TryParse(valorStr, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                        {
                            TempData["ErrorMessage"] = "Valor total inválido.";
                            return RedirectToAction("Pedidos");
                        }
                    }

                    pedido.ValorTotal = valor;
                    pedido.Status = status;
                    pedido.EnderecoEntrega = enderecoEntrega ?? "";
                    pedido.BairroEntrega = bairroEntrega ?? "";
                    pedido.Observacao = observacao ?? "";
                    pedido.ClienteId = clienteId;
                    
                    _context.Entry(pedido).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Pedido {id} atualizado com sucesso no banco.");
                    TempData["SuccessMessage"] = "Pedido #" + id + " atualizado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar pedido {id}");
                TempData["ErrorMessage"] = "Erro ao atualizar pedido: " + ex.Message;
            }
            return RedirectToAction("Pedidos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePedido(int id)
        {
            _logger.LogInformation($"Excluindo Pedido {id}");
            try 
            {
                var pedido = await _context.Vendas.FindAsync(id);
                if (pedido != null)
                {
                    _context.Vendas.Remove(pedido);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Pedido {id} removido com sucesso do banco.");
                    TempData["SuccessMessage"] = "Pedido removido com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir pedido {id}");
                TempData["ErrorMessage"] = "Erro ao excluir pedido: " + ex.Message;
            }
            return RedirectToAction("Pedidos");
        }

        // ================================
        // 🔥 PRODUTOS
        // ================================
        [HttpGet]
        public async Task<IActionResult> Produtos()
        {
            _logger.LogInformation("Acessando Produtos Admin");
            var produtos = await _produtoService.GetProdutosAsync();
            return View(produtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduto(string nome, string descricao, string precoVenda, int estoqueAtual, string imagemUrl)
        {
            _logger.LogInformation($"Criando Produto: {nome}");
            try
            {
                decimal preco = 0;
                if (!string.IsNullOrEmpty(precoVenda))
                {
                    string precoStr = precoVenda.Replace(",", ".");
                    decimal.TryParse(precoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out preco);
                }

                var produto = new Produto
                {
                    Nome = nome,
                    Descricao = descricao ?? "",
                    PrecoVenda = preco,
                    EstoqueAtual = estoqueAtual,
                    ImagemUrl = imagemUrl ?? "/images/produtos/default.png",
                    Ativo = true,
                    CategoriaId = 1,
                    PrecoCusto = 0
                };

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Produto {produto.Id} criado no banco.");
                TempData["SuccessMessage"] = "Produto criado com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto");
                TempData["ErrorMessage"] = "Erro ao criar produto: " + ex.Message;
            }
            return RedirectToAction("Produtos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduto(int id, string nome, string descricao, string precoVenda, int estoqueAtual, string imagemUrl)
        {
            _logger.LogInformation($"Editando Produto {id}");
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto != null)
                {
                    decimal preco = 0;
                    if (!string.IsNullOrEmpty(precoVenda))
                    {
                        string precoStr = precoVenda.Replace(",", ".");
                        decimal.TryParse(precoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out preco);
                    }

                    produto.Nome = nome;
                    produto.Descricao = descricao ?? "";
                    produto.PrecoVenda = preco;
                    produto.EstoqueAtual = estoqueAtual;
                    if (!string.IsNullOrEmpty(imagemUrl))
                        produto.ImagemUrl = imagemUrl;

                    _context.Entry(produto).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Produto {id} atualizado no banco.");
                    TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar produto {id}");
                TempData["ErrorMessage"] = "Erro ao atualizar produto: " + ex.Message;
            }
            return RedirectToAction("Produtos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            _logger.LogInformation($"Excluindo Produto {id}");
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto != null)
                {
                    _context.Produtos.Remove(produto);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Produto {id} removido do banco.");
                    TempData["SuccessMessage"] = "Produto excluído!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir produto {id}");
                TempData["ErrorMessage"] = "Erro ao excluir produto: " + ex.Message;
            }
            return RedirectToAction("Produtos");
        }

        // ================================
        // 🔥 CLIENTES (GESTÃO DE CLIENTES)
        // ================================
        [HttpGet]
        public async Task<IActionResult> Clientes()
        {
            _logger.LogInformation("Acessando Clientes Admin");
            var clientes = await _clienteService.GetClientesAsync();
            return View(clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCliente(string nome, string telefone, string saldoDevedor)
        {
            _logger.LogInformation($"Criando Cliente: Nome={nome}, Tel={telefone}");
            try
            {
                decimal saldo = 0;
                if (!string.IsNullOrEmpty(saldoDevedor))
                {
                    string saldoStr = saldoDevedor.Replace(",", ".");
                    decimal.TryParse(saldoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out saldo);
                }

                var cliente = new Cliente 
                { 
                    Nome = nome, 
                    Telefone = telefone, 
                    SaldoDevedor = saldo 
                };
                
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente {cliente.Id} criado no banco.");
                TempData["SuccessMessage"] = "Cliente criado com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente");
                TempData["ErrorMessage"] = "Erro ao criar cliente: " + ex.Message;
            }
            return RedirectToAction("Clientes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCliente(int id, string nome, string telefone, string saldoDevedor)
        {
            _logger.LogInformation($"Editando Cliente {id}");
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    decimal saldo = 0;
                    if (!string.IsNullOrEmpty(saldoDevedor))
                    {
                        string saldoStr = saldoDevedor.Replace(",", ".");
                        decimal.TryParse(saldoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out saldo);
                    }

                    cliente.Nome = nome;
                    cliente.Telefone = telefone;
                    cliente.SaldoDevedor = saldo;
                    
                    _context.Entry(cliente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Cliente {id} atualizado no banco.");
                    TempData["SuccessMessage"] = "Cliente atualizado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar cliente {id}");
                TempData["ErrorMessage"] = "Erro ao atualizar cliente: " + ex.Message;
            }
            return RedirectToAction("Clientes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            _logger.LogInformation($"Excluindo Cliente {id}");
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Cliente {id} removido do banco.");
                    TempData["SuccessMessage"] = "Cliente excluído!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir cliente {id}");
                TempData["ErrorMessage"] = "Erro ao excluir cliente: " + ex.Message;
            }
            return RedirectToAction("Clientes");
        }

        // Mantém suporte para usuários identity se necessário, mas em rotas diferentes
        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirUsuario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    TempData["ErrorMessage"] = "Não pode excluir admin!";
                    return RedirectToAction("Clientes");
                }

                await _userManager.DeleteAsync(user);
                TempData["SuccessMessage"] = "Usuário excluído!";
            }

            return RedirectToAction("Clientes");
        }

        // ================================
        // 🔥 EDITAR SITE
        // ================================
        [HttpGet]
        public async Task<IActionResult> EditarSite()
        {
            _logger.LogInformation("Acessando Editar Site Admin");
            var contents = await _context.SiteContents.ToListAsync();
            return View(contents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSiteContent(string key, string value, IFormFile? file)
        {
            try
            {
                var content = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == key);
                if (content != null)
                {
                    if (file != null && file.Length > 0)
                    {
                        var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "site");
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadDir, fileName);
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        content.Value = "/uploads/site/" + fileName;
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        content.Value = value;
                    }
                    
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Conteúdo atualizado com sucesso!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao atualizar conteúdo: " + ex.Message;
            }
            return RedirectToAction("EditarSite");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeedMockData()
        {
            try 
            {
                // Limpa tudo para garantir que o seed novo entre
                _context.Vendas.RemoveRange(_context.Vendas);
                _context.Clientes.RemoveRange(_context.Clientes);
                _context.SiteContents.RemoveRange(_context.SiteContents);
                await _context.SaveChangesAsync();

                await SeedMockDataInterno();
                
                // Adicionar dados fictícios para análise do CRUD
                var clientesFicticios = new List<Cliente>
                {
                    new Cliente { Nome = "João Silva", Telefone = "(11) 98888-7777", SaldoDevedor = 15.50m },
                    new Cliente { Nome = "Maria Oliveira", Telefone = "(11) 97777-6666", SaldoDevedor = 0.00m },
                    new Cliente { Nome = "Pedro Santos", Telefone = "(11) 96666-5555", SaldoDevedor = 45.00m }
                };
                _context.Clientes.AddRange(clientesFicticios);
                await _context.SaveChangesAsync();

                var clientes = await _context.Clientes.ToListAsync();
                var vendasFicticias = new List<Venda>
                {
                    new Venda { ValorTotal = 25.00m, Status = StatusVenda.Pendente, EnderecoEntrega = "Rua das Flores, 123", BairroEntrega = "Jardins", DataVenda = DateTime.Now.AddHours(-2), ClienteId = clientes[0].Id, FormaPagamento = TipoPagamento.Pix, Observacao = "Troco para 50" },
                    new Venda { ValorTotal = 18.50m, Status = StatusVenda.Preparando, EnderecoEntrega = "Av. Paulista, 1000", BairroEntrega = "Bela Vista", DataVenda = DateTime.Now.AddHours(-1), ClienteId = clientes[1].Id, FormaPagamento = TipoPagamento.Dinheiro, Observacao = "Sem campainha" },
                    new Venda { ValorTotal = 32.00m, Status = StatusVenda.Entregue, EnderecoEntrega = "Rua Augusta, 500", BairroEntrega = "Consolação", DataVenda = DateTime.Now.AddDays(-1), ClienteId = clientes[2].Id, FormaPagamento = TipoPagamento.Cartao, Observacao = "" }
                };
                _context.Vendas.AddRange(vendasFicticias);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Sistema resetado com dados fictícios de teste!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao resetar dados: " + ex.Message;
            }
            return RedirectToAction("EditarSite");
        }

        // ================================
        // 🔥 MÉTRICAS
        // ================================
        [HttpGet]
        public async Task<IActionResult> EditMetrics()
        {
            _logger.LogInformation("Acessando Editar Métricas Admin");
            
            var m_vendasHoje = _context.SiteContents.FirstOrDefault(c => c.Key == "Admin_Metrics_VendasHoje")?.Value;
            var m_totalClientes = _context.SiteContents.FirstOrDefault(c => c.Key == "Admin_Metrics_TotalClientes")?.Value;
            var m_faturamento = _context.SiteContents.FirstOrDefault(c => c.Key == "Admin_Metrics_Faturamento")?.Value;

            ViewBag.VendasHoje = m_vendasHoje ?? "0";
            ViewBag.TotalClientes = m_totalClientes ?? _context.Clientes.Count().ToString();
            
            if (decimal.TryParse(m_faturamento, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal f))
            {
                ViewBag.Faturamento = f.ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                var realFaturamento = _context.Vendas.Sum(v => (decimal?)v.ValorTotal) ?? 0;
                ViewBag.Faturamento = realFaturamento.ToString("F2", CultureInfo.InvariantCulture);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMetrics(string vendasHoje, string totalClientes, string faturamento)
        {
            _logger.LogInformation($"Atualizando Métricas: VendasHoje={vendasHoje}, TotalClientes={totalClientes}, Faturamento={faturamento}");
            
            try 
            {
                // Normaliza o faturamento para usar ponto decimal
                string normalizedFaturamento = faturamento.Replace(",", ".");
                if (!decimal.TryParse(normalizedFaturamento, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    TempData["ErrorMessage"] = "Formato de faturamento inválido.";
                    return RedirectToAction("EditMetrics");
                }

                await UpdateOrAddContent("Admin_Metrics_VendasHoje", vendasHoje);
                await UpdateOrAddContent("Admin_Metrics_TotalClientes", totalClientes);
                await UpdateOrAddContent("Admin_Metrics_Faturamento", normalizedFaturamento);

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Métricas atualizadas com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar métricas");
                TempData["ErrorMessage"] = "Erro ao salvar métricas: " + ex.Message;
                return RedirectToAction("EditMetrics");
            }
        }

        private async Task UpdateOrAddContent(string key, string value)
        {
            var content = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == key);
            if (content != null)
            {
                content.Value = value;
            }
            else
            {
                _context.SiteContents.Add(new SiteContent 
                { 
                    Key = key, 
                    Value = value, 
                    Page = "Admin", 
                    Type = "Metric" 
                });
            }
        }

        private async Task SeedMockDataInterno()
        {
            if (!_context.SiteContents.Any())
            {
                var contents = new List<SiteContent>
                {
                    // Início
                    new SiteContent { Key = "Home_Hero_Title", Value = "AÇAÍ GALÁCTICO", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Subtitle", Value = "Explore o universo de sabores do melhor açaí da região!", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Hero_Background", Value = "/images/galaxiaa.jpg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_About_Title", Value = "Quem Somos?", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_About_Text", Value = "No Açaí Galáctico, somos uma pequena loja familiar apaixonada por entregar sabor e qualidade.", Page = "Inicio", Type = "Description" },
                    new SiteContent { Key = "Home_About_Image", Value = "/images/entrada-acai.jpeg", Page = "Inicio", Type = "Image" },
                    
                    // Exposição Home
                    new SiteContent { Key = "Home_Expo_Img1", Value = "/images/acai-expo-1.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title1", Value = "Açaí com Galak", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Expo_Img2", Value = "/images/acai-expo-2.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title2", Value = "Açaí com morango e Nutella", Page = "Inicio", Type = "Text" },
                    new SiteContent { Key = "Home_Expo_Img3", Value = "/images/acai-expo-3.jpeg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Expo_Title3", Value = "Açaí com M&M", Page = "Inicio", Type = "Text" },

                    // Cardápio
                    new SiteContent { Key = "Menu_Background_Image", Value = "/images/galaxiaa.jpg", Page = "Cardapio", Type = "Image" },
                    new SiteContent { Key = "Menu_Tradicional_Title", Value = "Tradicional", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_300ml", Value = "10.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_400ml", Value = "12.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_500ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Price_700ml", Value = "18.00", Page = "Cardapio", Type = "Text" },
                    
                    new SiteContent { Key = "Menu_Trufado_Title", Value = "Trufado", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Subtitle", Value = "(creme de chocolate)", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_300ml", Value = "13.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_400ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_500ml", Value = "17.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Trufado_Price_700ml", Value = "22.00", Page = "Cardapio", Type = "Text" },

                    new SiteContent { Key = "Menu_Gourmet_Title", Value = "Gourmet", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Subtitle", Value = "(Nutella, Galak com Negresco)", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_300ml", Value = "15.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_400ml", Value = "17.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_500ml", Value = "20.00", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Gourmet_Price_700ml", Value = "25.00", Page = "Cardapio", Type = "Text" },

                    // Limites Cardápio
                    new SiteContent { Key = "Menu_Limit_300ml_Fruits", Value = "1", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_300ml_Toppings", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_400ml_Fruits", Value = "1", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_400ml_Toppings", Value = "3", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_500ml_Fruits", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_500ml_Toppings", Value = "4", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_700ml_Fruits", Value = "2", Page = "Cardapio", Type = "Text" },
                    new SiteContent { Key = "Menu_Limit_700ml_Toppings", Value = "5", Page = "Cardapio", Type = "Text" },

                    // Fazer Pedido
                    new SiteContent { Key = "Order_Side_Title", Value = "Sabor de Outro Mundo", Page = "FazerPedido", Type = "Text" },
                    new SiteContent { Key = "Order_Welcome_Text", Value = "Monte o açaí perfeito para sua viagem galáctica.", Page = "FazerPedido", Type = "Text" },
                    new SiteContent { Key = "Order_Background_Image", Value = "/images/galaxiaa.jpg", Page = "FazerPedido", Type = "Image" },
                    new SiteContent { Key = "Order_Side_Image", Value = "/images/acai-expo-1.jpeg", Page = "FazerPedido", Type = "Image" },
                    new SiteContent { Key = "Order_Delivery_Note", Value = "Prezado cliente, informamos que no momento não dispomos de entregadores próprios. Recomendamos solicitar a retirada via Uber Moto.", Page = "FazerPedido", Type = "Description" },
                    new SiteContent { Key = "Order_Delivery_Link", Value = "https://m.uber.com/", Page = "FazerPedido", Type = "Text" },
                    
                    // Configurações de Pedido (Frutas e Acompanhamentos)
                    new SiteContent { Key = "Order_Fruits", Value = "Banana\nMorango\nManga\nKiwi\nAbacaxi\nUva", Page = "FazerPedido", Type = "Description" },
                    new SiteContent { Key = "Order_Toppings", Value = "Creme de Paçoca|Uma camada deliciosa de creme de amendoim.|/images/logo-sem-escrita.png\nCreme de Pitaya|Sabor exótico e cor vibrante.|/images/logo-sem-escrita.png\nCreme de Leite em Pó|Cremosidade extra para seu açaí.|/images/logo-sem-escrita.png\nLeite em Pó|Clássico indispensável.|/images/logo-sem-escrita.png\nGranola|Crocância saudável.|/images/logo-sem-escrita.png\nPaçoca|O sabor do amendoim.|/images/logo-sem-escrita.png", Page = "FazerPedido", Type = "Description" },

                    new SiteContent { Key = "Contato_Telefone", Value = "(11) 98259-9249", Page = "Contato", Type = "Text" },
                    new SiteContent { Key = "Contato_Endereco", Value = "Rua das Estrelas, 123 - Centro", Page = "Contato", Type = "Description" },

                    // Extras Home
                    new SiteContent { Key = "Home_Others_Image", Value = "/images/produtos.jpg", Page = "Inicio", Type = "Image" },
                    new SiteContent { Key = "Home_Others_Title", Value = "BEBIDAS, SORVETES E SALGADOS", Page = "Inicio", Type = "Text" }
                };
                _context.SiteContents.AddRange(contents);
                await _context.SaveChangesAsync();
            }
        }

        private async Task EnsureSiteContentsUpToDate()
        {
            var contents = _context.SiteContents.ToList();
            if (!contents.Any())
            {
                await SeedMockDataInterno();
            }
        }
    }
}