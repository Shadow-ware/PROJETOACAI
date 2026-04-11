using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.ViewModels
{
    public class PerfilViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string NewUserName { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string NewEmail { get; set; }

        [Display(Name = "Telefone")]
        [RegularExpression(@"^\(\d{2}\) \d{4,5}-\d{4}$", ErrorMessage = "Telefone inválido. Use o formato (00) 00000-0000")]
        public string? Telefone { get; set; }

        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [Display(Name = "CEP")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "CEP inválido. Use o formato 00000-000")]
        public string? Cep { get; set; }

        [Display(Name = "Senha Atual")]
        public string? CurrentPassword { get; set; }

        [Display(Name = "Nova Senha")]
        [MinLength(4, ErrorMessage = "A nova senha deve ter pelo menos 4 caracteres")]
        public string? NewPassword { get; set; }

        [Display(Name = "Foto de Perfil")]
        public IFormFile? Photo { get; set; }

        public string? CurrentPhotoUrl { get; set; }
    }
}
