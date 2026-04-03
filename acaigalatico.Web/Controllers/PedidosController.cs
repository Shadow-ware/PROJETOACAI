using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using acaigalatico.Domain.Entities;
using acaigalatico.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acaigalatico.Web.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PedidosController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class PedidoVm
        {
            public int Numero { get; set; }
            public System.DateTime Data { get; set; }
            public string Status { get; set; } = string.Empty;
            public decimal ValorTotal { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = await _userManager.GetEmailAsync(identityUser) ?? identityUser.Email ?? string.Empty;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var identificadorCliente = BuildClientIdentifier(identityUser, usuario);

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Telefone == identificadorCliente);
            var pedidos = new List<PedidoVm>();

            if (cliente != null)
            {
                pedidos = await _context.Vendas
                    .AsNoTracking()
                    .Where(v => v.ClienteId == cliente.Id)
                    .OrderByDescending(v => v.DataVenda)
                    .Select(v => new PedidoVm
                    {
                        Numero = v.Id,
                        Data = v.DataVenda,
                        Status = FormatStatus(v.Status),
                        ValorTotal = v.ValorTotal
                    })
                    .ToListAsync();
            }

            return View(pedidos);
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

        private static string FormatStatus(StatusVenda status)
        {
            return status switch
            {
                StatusVenda.Pendente => "Novo pedido",
                StatusVenda.Preparando => "Em andamento",
                StatusVenda.SaiuParaEntrega => "Saiu para entrega",
                StatusVenda.Entregue => "Entregue",
                StatusVenda.Cancelado => "Cancelado",
                _ => "Novo pedido"
            };
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
