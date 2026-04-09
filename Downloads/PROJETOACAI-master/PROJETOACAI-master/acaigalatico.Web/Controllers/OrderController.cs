using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> contents;
            try
            {
                contents = await _context.SiteContents
                    .AsNoTracking()
                    .Where(c => c.Page == "FazerPedido" || c.Page == "Cardapio")
                    .ToDictionaryAsync(c => c.Key, c => c.Value);
            }
            catch
            {
                contents = new Dictionary<string, string>();
            }

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
        public IActionResult SubmitOrder(string type, string size, int quantity, List<string> fruits, List<string> toppings)
        {
            // Logic to process order would go here (save to DB, etc.)
            // For now, redirect to a Success page or back to Home with a message.
            TempData["SuccessMessage"] = "Pedido realizado com sucesso! Em breve entregaremos sua galáxia de sabor.";
            return RedirectToAction("Index", "Home");
        }
    }
}
