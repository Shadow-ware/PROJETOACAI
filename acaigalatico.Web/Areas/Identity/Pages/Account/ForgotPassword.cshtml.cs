using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly AppDbContext _context;

        public ForgotPasswordModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Email { get; set; }
        public string EmailMascarado { get; set; }

        public int Etapa { get; set; }

        public class InputModel
        {
            [Display(Name = "Nome Completo")]
            public string Nome { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            // 1️⃣ tenta pegar da URL
            if (!string.IsNullOrEmpty(email))
            {
                Email = email;
            }
            else
            {
                // 2️⃣ fallback para TempData
                Email = TempData["EmailRecuperacao"]?.ToString();
            }

            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError(string.Empty, "Email não identificado. Tente o login primeiro.");
                Etapa = 0;
                return Page();
            }

            // Validar se o email existe no banco
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Email == Email);
            
            if (!usuarioExiste)
            {
                ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                Etapa = 0;
                return Page();
            }

            EmailMascarado = MascaraEmail(Email);
            
            if (TempData.ContainsKey("Etapa"))
            {
                Etapa = (int)TempData["Etapa"];
            }
            else
            {
                Etapa = 1;
            }
            
            TempData["EmailRecuperacao"] = Email;
            TempData.Keep("EmailRecuperacao");
            TempData["Etapa"] = Etapa;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? etapaInformada)
        {
            Email = TempData["EmailRecuperacao"]?.ToString();
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError(string.Empty, "Sessão expirada. Tente logar novamente.");
                return Page();
            }

            EmailMascarado = MascaraEmail(Email);
            
            // Determina a etapa atual (vinda do formulário ou do TempData)
            int etapaAtual = etapaInformada ?? (TempData.ContainsKey("Etapa") ? (int)TempData["Etapa"] : 1);

            if (etapaAtual == 1)
            {
                // ETAPA 1 finalizada -> Mudar para ETAPA 2
                Etapa = 2;
                TempData["Etapa"] = 2;
                TempData["EmailRecuperacao"] = Email;
                TempData.Keep("EmailRecuperacao");
                return Page();
            }
            else if (etapaAtual == 2)
            {
                Etapa = 2; // Mantém na etapa 2 em caso de erro

                if (string.IsNullOrWhiteSpace(Input.Nome))
                {
                    ModelState.AddModelError("Input.Nome", "O nome completo é obrigatório.");
                    TempData["Etapa"] = 2;
                    TempData["EmailRecuperacao"] = Email;
                    TempData.Keep("EmailRecuperacao");
                    return Page();
                }

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == Email);

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário não encontrado");
                    TempData["Etapa"] = 2;
                    TempData["EmailRecuperacao"] = Email;
                    TempData.Keep("EmailRecuperacao");
                    return Page();
                }

                if (usuario.Nome.Trim().ToLower() != Input.Nome.Trim().ToLower())
                {
                    ModelState.AddModelError(string.Empty, "Nome não confere");
                    TempData["Etapa"] = 2;
                    TempData["EmailRecuperacao"] = Email;
                    TempData.Keep("EmailRecuperacao");
                    return Page();
                }

                TempData["EmailRecuperacao"] = Email; // Manter para o próximo passo
                return RedirectToPage("./ResetPassword");
            }

            return Page();
        }

        public string MascaraEmail(string email)
        {
            var partes = email.Split('@');
            var nome = partes[0];

            if (nome.Length <= 3)
                return "***@" + partes[1];

            return nome.Substring(0, 3) + "****@" + partes[1];
        }
    }
}
