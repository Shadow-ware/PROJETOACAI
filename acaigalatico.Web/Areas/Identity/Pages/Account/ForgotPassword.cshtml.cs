using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using acaigalatico.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Informe seu email.")]
            [EmailAddress(ErrorMessage = "Digite um email valido.")]
            public string Email { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            try
            {
                var message =
                    $"Ola, {user.UserName}!<br/><br/>" +
                    "Recebemos uma solicitacao para redefinir sua senha.<br/>" +
                    $"Clique no link abaixo para continuar:<br/><br/><a href=\"{callbackUrl}\">Redefinir senha</a>";

                await _emailService.SendEmailAsync(Input.Email, "Redefinicao de senha - Acai Galactico", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar email de redefinicao para {Email}.", Input.Email);
                TempData["WarningMessage"] = "Nao foi possivel enviar o email agora. Verifique a configuracao do envio.";
            }

            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
