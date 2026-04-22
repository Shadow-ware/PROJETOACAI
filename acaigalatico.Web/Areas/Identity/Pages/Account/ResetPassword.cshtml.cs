using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace acaigalatico.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly AppDbContext _context;

        public ResetPasswordModel(
            UserManager<IdentityUser> userManager,
            IPasswordHasher<IdentityUser> passwordHasher,
            AppDbContext context)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação não conferem.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            var email = TempData["EmailRecuperacao"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("./Login");
            }
            TempData.Keep("EmailRecuperacao");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var email = TempData["EmailRecuperacao"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("./Login");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Tenta por username se não achou por email (já que no login ele tenta ambos)
                    user = await _userManager.FindByNameAsync(email);
                }

                if (user != null)
                {
                    // 1️⃣2️⃣ ATUALIZAR SENHA
                    user.PasswordHash = _passwordHasher.HashPassword(user, Input.Password);
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // Opcional: Sincronizar com a tabela Usuarios se necessário
                        // Embora SenhaHash lá pareça ser apenas placeholder
                        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                        if (usuario != null)
                        {
                            usuario.SenhaHash = "********"; // Mantendo o padrão do projeto
                            _context.Update(usuario);
                            await _context.SaveChangesAsync();
                        }

                        return RedirectToPage("./Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Erro ao localizar usuário para reset.");
                }
            }

            return Page();
        }
    }
}
