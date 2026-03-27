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

namespace acaigalatico.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly acaigalatico.Infrastructure.Context.AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(
            IProdutoService produtoService,
            acaigalatico.Infrastructure.Context.AppDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _produtoService = produtoService;
            _context = context;
            _userManager = userManager;
        }

        // ================================
        // 🔥 INDEX CORRIGIDO -Nicolas
        // Remove erro do SeedMockData()
        // ================================
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!_context.SiteContents.Any())
                {
                    await SeedMockDataInterno(); // -Nicolas corrigido
                }
                else
                {
                    await EnsureSiteContentsUpToDate();
                }

                var products = await _produtoService.GetProdutosAsync();

                ViewBag.TotalClientes = _context.Clientes.Count();
                ViewBag.Faturamento = _context.Vendas.Sum(v => (decimal?)v.ValorTotal) ?? 0;

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro: " + ex.Message;
                return View();
            }
        }

        // ================================
        // 🔥 CLIENTES (USUÁRIOS REAIS) -Nicolas
        // ================================
        [HttpGet]
        public async Task<IActionResult> Clientes()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            return View(usuarios);
        }

        // ================================
        // 🔥 EXCLUIR USUÁRIO -Nicolas
        // ================================
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
            }

            return RedirectToAction("Clientes");
        }

        // ================================
        // 🔥 MÉTODO INTERNO (CORREÇÃO) -Nicolas
        // ================================
        private async Task SeedMockDataInterno()
        {
            if (!_context.Clientes.Any())
            {
                _context.Clientes.Add(new Cliente { Nome = "Cliente Teste", Telefone = "11999999999" });
            }

            if (!_context.Vendas.Any())
            {
                _context.Vendas.Add(new Venda
                {
                    ValorTotal = 20,
                    DataVenda = DateTime.Now,
                    Status = StatusVenda.Entregue,
                    EnderecoEntrega = "Teste",
                    BairroEntrega = "Centro",
                    FormaPagamento = TipoPagamento.Dinheiro
                });
            }

            await _context.SaveChangesAsync();
        }

        // ================================
        // 🔥 MÉTODO JÁ EXISTENTE (OK)
        // ================================
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