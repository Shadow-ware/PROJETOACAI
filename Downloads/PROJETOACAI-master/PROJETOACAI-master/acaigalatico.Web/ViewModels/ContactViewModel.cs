using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.ViewModels;

public class ContactViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(70, ErrorMessage = "O nome deve ter no máximo 70 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
    [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O assunto é obrigatório.")]
    public string Assunto { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [StringLength(1000, ErrorMessage = "A mensagem deve ter no máximo 1000 caracteres.")]
    public string Mensagem { get; set; } = string.Empty;
}
