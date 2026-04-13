using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            // ========================= 🔥 ALTERAÇÃO 🔥 =========================
            // Campo único, mas comportamento muda dependendo se é admin ou cliente
            [Required]
            [Display(Name = "Login")]
            public string Login { get; set; }
            // ====================================================================

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lembrar-me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                IdentityUser user = null;

                // ========================= 🔥 ALTERAÇÃO PRINCIPAL 🔥 =========================
                // Aqui diferenciamos CLIENTE vs ADMIN usando a query ?isAdmin=true

                bool isAdminLogin = Request.Query["isAdmin"] == "true";

                if (isAdminLogin)
                {
                    // 🔐 ADMIN → só username
                    user = await _userManager.FindByNameAsync(Input.Login);
                }
                else
                {
                    // 👤 CLIENTE → primeiro tenta por Email, se não der, tenta por UserName
                    user = await _userManager.FindByEmailAsync(Input.Login);
                    if (user == null)
                    {
                        user = await _userManager.FindByNameAsync(Input.Login);
                    }
                }

                // ============================================================================

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        Input.Password,
                        Input.RememberMe,
                        lockoutOnFailure: false
                    );

                    if (result.Succeeded)
                    {
                        // ========================= 🔥 ADMIN REDIRECT 🔥 =========================
                        if (isAdminLogin)
                            return RedirectToAction("Index", "Admin", new { area = "" });
                        // =======================================================================

                        return LocalRedirect(returnUrl);
                    }
                }

                ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
                return Page();
            }

            return Page();
        }
    }
}