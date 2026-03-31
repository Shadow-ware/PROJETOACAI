using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class SettingsModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Theme { get; set; } = "dark";

            [Required]
            public string Language { get; set; } = "pt-BR";
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            TempData["SettingsSaved"] = "Configurações salvas.";
            return RedirectToPage();
        }
    }
}
