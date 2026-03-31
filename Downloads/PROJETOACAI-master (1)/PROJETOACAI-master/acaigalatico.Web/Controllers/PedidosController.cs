using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        public class PedidoVm
        {
            public int Numero { get; set; }
            public System.DateTime Data { get; set; }
            public string Status { get; set; }
            public decimal ValorTotal { get; set; }
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pedidos = new List<PedidoVm>
            {
                new PedidoVm { Numero = 1023, Data = System.DateTime.Today.AddDays(-2), Status = "Entregue", ValorTotal = 38.50m },
                new PedidoVm { Numero = 1041, Data = System.DateTime.Today.AddDays(-1), Status = "Em preparo", ValorTotal = 28.00m },
                new PedidoVm { Numero = 1048, Data = System.DateTime.Today, Status = "Recebido", ValorTotal = 20.00m }
            };
            return View(pedidos);
        }
    }
}
