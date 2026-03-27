using acaigalatico.Application.Interfaces;
using acaigalatico.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;
        private readonly AppDbContext _context;

        public ProductsController(IProdutoService produtoService, ICategoriaService categoriaService, AppDbContext context)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> contents;
            try
            {
                contents = await _context.SiteContents
                    .AsNoTracking()
                    .Where(c => c.Page == "Cardapio")
                    .ToDictionaryAsync(c => c.Key, c => c.Value);
            }
            catch
            {
                contents = new Dictionary<string, string>();
            }
                
            return View(contents);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoriaId = new SelectList(await _categoriaService.GetCategoriasAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(acaigalatico.Application.DTOs.ProdutoDTO produtoDto)
        {
            if (ModelState.IsValid)
            {
                await _produtoService.AddAsync(produtoDto);
                TempData["SuccessMessage"] = "Produto criado com sucesso!";
                return RedirectToAction("Produtos", "Admin");
            }
            ViewBag.CategoriaId = new SelectList(await _categoriaService.GetCategoriasAsync(), "Id", "Nome", produtoDto.CategoriaId);
            return View(produtoDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produtoDto = await _produtoService.GetByIdAsync(id);

            if (produtoDto == null) return NotFound();

            ViewBag.CategoriaId = new SelectList(await _categoriaService.GetCategoriasAsync(), "Id", "Nome", produtoDto.CategoriaId);
            return View(produtoDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(acaigalatico.Application.DTOs.ProdutoDTO produtoDto)
        {
            if (ModelState.IsValid)
            {
                await _produtoService.UpdateAsync(produtoDto);
                TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                return RedirectToAction("Produtos", "Admin");
            }
            ViewBag.CategoriaId = new SelectList(await _categoriaService.GetCategoriasAsync(), "Id", "Nome", produtoDto.CategoriaId);
            return View(produtoDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var produtoDto = await _produtoService.GetByIdAsync(id);
            if (produtoDto == null) return NotFound();
            return View(produtoDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _produtoService.RemoveAsync(id);
            TempData["SuccessMessage"] = "Produto removido com sucesso!";
            return RedirectToAction("Produtos", "Admin");
        }
    }
}
