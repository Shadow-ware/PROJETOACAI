using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.ViewModels
{
    public class PerfilViewModel
    {
        [Required]
        public string NewUserName { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public IFormFile Photo { get; set; }

        public string CurrentPhotoUrl { get; set; }
    }
}
