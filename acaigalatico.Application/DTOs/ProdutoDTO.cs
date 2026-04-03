using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Application.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço de custo é obrigatório.")]
        [Display(Name = "Preço de Custo")]
        public decimal PrecoCusto { get; set; }

        [Required(ErrorMessage = "O preço de venda é obrigatório.")]
        [Display(Name = "Preço de Venda")]
        public decimal? PrecoVenda { get; set; }

        [Display(Name = "Estoque Atual")]
        public int EstoqueAtual { get; set; }

        [Display(Name = "Estoque Mínimo")]
        public int EstoqueMinimo { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [StringLength(255, ErrorMessage = "A URL da imagem deve ter no máximo 255 caracteres.")]
        [Display(Name = "URL da Imagem")]
        public string? ImagemUrl { get; set; }

        [Required(ErrorMessage = "O ID da categoria é obrigatório.")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Categoria")]
        public string? CategoriaNome { get; set; }
    }
}
