using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet]
        public IActionResult Index()
        {
            var items = new List<CartItem>
            {
                new CartItem { Nome = "Açaí Tradicional 500ml", ImagemUrl = "/images/acai-expo-1.jpeg", Preco = 20.00m, Quantidade = 1 },
                new CartItem { Nome = "Açaí Gourmet 700ml", ImagemUrl = "/images/acai-expo-2.jpeg", Preco = 28.50m, Quantidade = 2 }
            };
            return View(items);
        }
    }
}
