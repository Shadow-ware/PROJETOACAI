using acaigalatico.Domain.Entities;
using acaigalatico.Application.Interfaces;
using acaigalatico.Web.Models;
using System.Net;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace acaigalatico.Web.Controllers
{
    [AllowAnonymous]
public class AdminController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly IClienteService _clienteService;
        private readonly IVendaService _vendaService;
        private readonly IConteudoSiteService _conteudoService;
        private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _environment;

        public AdminController(
            IProdutoService produtoService,
            IClienteService clienteService,
            IVendaService vendaService,
            IConteudoSiteService conteudoService,
            acaigalatico.Infrastructure.Context.AppDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<AdminController> logger,
            IWebHostEnvironment environment)
        {
            _produtoService = produtoService;
            _clienteService = clienteService;
            _vendaService = vendaService;
            _conteudoService = conteudoService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var hoje = DateTime.Now.Date;
            var seteDiasAtras = hoje.AddDays(-7);

            _logger.LogInformation("Dashboard Index acessado. Hoje: {Hoje}", hoje.ToString("dd/MM/yyyy"));

            // 1. Vendas Hoje (Prioriza manual se existir)
            var m_vendasHoje = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_VendasHoje");
            if (m_vendasHoje != null)
                ViewBag.VendasHoje = m_vendasHoje.Value;
            else
                ViewBag.VendasHoje = await _context.Vendas.CountAsync(v => v.DataVenda.Date == hoje);

            // 2. Total Clientes (Prioriza manual se existir)
            var m_totalClientes = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_TotalClientes");
            if (m_totalClientes != null)
                ViewBag.TotalClientes = m_totalClientes.Value;
            else
                ViewBag.TotalClientes = await _context.Clientes.CountAsync();

            // 3. Faturamento (Prioriza manual se existir)
            var m_faturamento = await _context.SiteContents.FirstOrDefaultAsync(c => c.Key == "Admin_Metrics_Faturamento");
            if (m_faturamento != null && decimal.TryParse(m_faturamento.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal f))
                ViewBag.Faturamento = f;
            else
                ViewBag.Faturamento = await _context.Vendas.SumAsync(v => (decimal?)v.ValorTotal) ?? 0;

            // 4. Dados para o gráfico de 7 dias (Sincronizado com a lógica do EditChart)
            var labels = new List<string>();
            var data = new List<decimal>();

            for (int i = 6; i >= 0; i--)
            {
                var date = hoje.AddDays(-i);
                var label = date.ToString("dd/MM");
                labels.Add(label);

                // Tenta buscar valor manual no SiteContents
                var manualValue = await _context.SiteContents
                    .FirstOrDefaultAsync(c => c.Key == $"Admin_Chart_Value_{label}");

                if (manualValue != null && decimal.TryParse(manualValue.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                {
                    _logger.LogInformation("Valor manual encontrado para {Label}: {Valor}", label, val);
                    data.Add(val);
                }
                else
                {
                    // Fallback para dados reais do banco
                    var realValue = await _context.Vendas
                        .Where(v => v.DataVenda.Date == date)
                        .SumAsync(v => (decimal?)v.ValorTotal) ?? 0;
                    _logger.LogInformation("Valor real usado para {Label}: {Valor}", label, realValue);
                    data.Add(realValue);
                }
            }

            ViewBag.SalesLabels = labels;
            ViewBag.SalesData = data;
            
            // BUSCAR ÚLTIMOS PEDIDOS PARA O DASHBOARD
            ViewBag.UltimosPedidos = await _context.Vendas
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.DataVenda)
                .Take(5)
                .ToListAsync();

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
        public async Task<IActionResult> CreateProduto(string nome, string descricao, string precoVenda, int estoqueAtual, string? imagemUrl, IFormFile? file)
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

                string imgPath = imagemUrl ?? "/images/produtos/default.png";
                if (file != null && file.Length > 0)
                {
                    imgPath = await _conteudoService.SaveImageAsync(file);
                }

                var produto = new Produto
                {
                    Nome = nome,
                    Descricao = descricao ?? "",
                    PrecoVenda = preco,
                    EstoqueAtual = estoqueAtual,
                    ImagemUrl = imgPath,
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
            
            // Redireciona para onde veio (ou default)
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("EditarSite")) return RedirectToAction("EditarSite");
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
            
            var referer = Request.Headers["Referer"].ToString();
            if (referer.Contains("EditarSite")) return RedirectToAction("EditarSite");
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
        public IActionResult EditarSite()
        {
            var model = _conteudoService.GetInicio();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditarSite(ConteudoInicio model, List<IFormFile?> itemFiles, IFormFile? sobreImgFile, IFormFile? outrosImgFile)
        {
            try
            {
                // Upload da imagem principal (Hero)
                var heroFile = Request.Form.Files["heroFile"];
                if (heroFile != null && heroFile.Length > 0)
                {
                    model.ImagemUrl = await SaveUploadedFile(heroFile);
                }

                // Upload da imagem "Sobre Nós"
                if (sobreImgFile != null && sobreImgFile.Length > 0)
                {
                    model.SobreImagemUrl = await SaveUploadedFile(sobreImgFile);
                }

                // Upload da imagem "Outros Produtos"
                if (outrosImgFile != null && outrosImgFile.Length > 0)
                {
                    model.OutrosImagemUrl = await SaveUploadedFile(outrosImgFile);
                }

                // Upload das imagens dos itens de exposição
                for (int i = 0; i < model.ItensExposicao.Count; i++)
                {
                    var itemFile = itemFiles != null && itemFiles.Count > i ? itemFiles[i] : null;
                    if (itemFile != null && itemFile.Length > 0)
                    {
                        model.ItensExposicao[i].ImagemUrl = await SaveUploadedFile(itemFile);
                    }
                }

                _conteudoService.UpdateInicio(model);
                TempData["SuccessMessage"] = "Página inicial atualizada com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar site");
                TempData["ErrorMessage"] = "Erro ao atualizar: " + ex.Message;
            }

            return RedirectToAction("EditarSite");
        }

        // --- EDITAR CARDÁPIO ---
        [HttpGet]
        public IActionResult EditarCardapio()
        {
            var model = _conteudoService.GetCardapioConteudo();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCardapio(ConteudoCardapio model, IFormFile? backgroundFile)
        {
            try
            {
                if (backgroundFile != null && backgroundFile.Length > 0)
                {
                    model.BackgroundImage = await SaveUploadedFile(backgroundFile);
                }

                _conteudoService.UpdateCardapioConteudo(model);
                TempData["SuccessMessage"] = "Cardápio atualizado com sucesso!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cardápio");
                TempData["ErrorMessage"] = "Erro ao atualizar: " + ex.Message;
            }

            return RedirectToAction("EditarCardapio");
        }

        [HttpPost]
        public async Task<IActionResult> CriarItemCardapio(ItemCardapio item, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                item.ImagemUrl = await SaveUploadedFile(file);
            }
            else
            {
                item.ImagemUrl = "/images/produtos/default.png";
            }

            _conteudoService.AddItemCardapio(item);
            TempData["SuccessMessage"] = "Item adicionado ao cardápio!";
            return RedirectToAction("EditarCardapio");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarItemCardapio(ItemCardapio item, IFormFile? file)
        {
            var itemExistente = _conteudoService.GetItemCardapioById(item.Id);
            if (itemExistente != null)
            {
                if (file != null && file.Length > 0)
                {
                    item.ImagemUrl = await SaveUploadedFile(file);
                }
                else
                {
                    item.ImagemUrl = itemExistente.ImagemUrl;
                }

                _conteudoService.UpdateItemCardapio(item);
                TempData["SuccessMessage"] = "Item atualizado com sucesso!";
            }
            return RedirectToAction("EditarCardapio");
        }

        [HttpPost]
        public IActionResult ExcluirItemCardapio(Guid id)
        {
            _conteudoService.DeleteItemCardapio(id);
            TempData["SuccessMessage"] = "Item removido do cardápio!";
            return RedirectToAction("EditarCardapio");
        }

        private async Task<string> SaveUploadedFile(IFormFile file)
        {
            var uploadDir = Path.Combine(_environment.WebRootPath, "uploads", "site");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/site/" + fileName;
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

        // ================================
        // 🔥 CONFIGURAR GRÁFICO
        // ================================
        [HttpGet]
        public async Task<IActionResult> EditChart()
        {
            _logger.LogInformation("Acessando Editar Gráfico Admin");

            var hoje = DateTime.Now.Date;
            var labels = new List<string>();
            var data = new List<decimal>();

            for (int i = 6; i >= 0; i--)
            {
                var date = hoje.AddDays(-i);
                var label = date.ToString("dd/MM");
                labels.Add(label);

                // Tenta buscar valor manual no SiteContents
                var manualValue = await _context.SiteContents
                    .FirstOrDefaultAsync(c => c.Key == $"Admin_Chart_Value_{label}");

                if (manualValue != null && decimal.TryParse(manualValue.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                {
                    data.Add(val);
                }
                else
                {
                    // Fallback para dados reais do banco
                    var realValue = await _context.Vendas
                        .Where(v => v.DataVenda.Date == date)
                        .SumAsync(v => (decimal?)v.ValorTotal) ?? 0;
                    data.Add(realValue);
                }
            }

            ViewBag.SalesLabels = labels;
            ViewBag.SalesData = data;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateChart(List<string> labels, List<string> values)
        {
            _logger.LogInformation("Atualizando Dados do Gráfico. Recebidos {Count} labels e {ValCount} valores.", labels?.Count ?? 0, values?.Count ?? 0);

            try
            {
                if (labels == null || values == null || labels.Count != values.Count)
                {
                    _logger.LogWarning("Dados inválidos recebidos no UpdateChart.");
                    TempData["ErrorMessage"] = "Dados inválidos.";
                    return RedirectToAction("EditChart");
                }

                for (int i = 0; i < labels.Count; i++)
                {
                    var label = labels[i];
                    var valueStr = values[i].Replace(",", ".");
                    
                    _logger.LogInformation("Processando Dia {Label}: Valor original={Original}, Valor processado={Processed}", label, values[i], valueStr);

                    if (decimal.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    {
                        await UpdateOrAddContent($"Admin_Chart_Value_{label}", valueStr);
                    }
                    else 
                    {
                        _logger.LogWarning("Valor inválido para {Label}: {Value}", label, values[i]);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gráfico atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar gráfico");
                TempData["ErrorMessage"] = "Erro ao salvar dados do gráfico: " + ex.Message;
                return RedirectToAction("EditChart");
            }
        }

        private async Task SeedMockDataInterno()
        {
            await _conteudoService.SeedInitialDataAsync();
        }
    }
}