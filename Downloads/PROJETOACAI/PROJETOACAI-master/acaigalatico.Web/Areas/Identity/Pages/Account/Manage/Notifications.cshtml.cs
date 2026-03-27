using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class NotificationsModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public bool Promotions { get; set; } = true;
            public bool OrderStatus { get; set; } = true;
            public bool MenuNews { get; set; } = true;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            TempData["NotificationsSaved"] = "Preferências de notificação salvas.";
            return RedirectToPage();
        }
    }
}
