using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Application.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }
    }
}
