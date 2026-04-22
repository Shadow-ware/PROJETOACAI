using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using acaigalatico.Domain.Entities;
using acaigalatico.Infrastructure.Context;

namespace acaigalatico.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly AppDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Nome Completo")]
            public string Nome { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação não conferem.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (TempData.ContainsKey("Registro_Nome") || TempData.ContainsKey("Registro_Email"))
            {
                Input = new InputModel
                {
                    Nome = TempData["Registro_Nome"]?.ToString(),
                    Email = TempData["Registro_Email"]?.ToString()
                };
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // ========================= 🔥 ALTERAÇÃO AQUI 🔥 =========================
                // Antes: UserName = Email
                // Agora: UserName = Nome do usuário

                var user = new IdentityUser
                {
                    UserName = Input.Nome, // <-- AGORA usamos o NOME
                    Email = Input.Email
                };
                // ======================================================================

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Identity user created.");

                    try
                    {
                        // Continua salvando corretamente na sua tabela personalizada
                        var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Email == Input.Email);

                        if (usuarioExistente == null)
                        {
                            var usuario = new Usuario
                            {
                                Nome = Input.Nome,
                                Email = Input.Email,
                                SenhaHash = "********"
                            };

                            _context.Usuarios.Add(usuario);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao sincronizar usuário com a tabela Usuarios.");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            }

            TempData["Registro_Nome"] = Input?.Nome;
            TempData["Registro_Email"] = Input?.Email;
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}