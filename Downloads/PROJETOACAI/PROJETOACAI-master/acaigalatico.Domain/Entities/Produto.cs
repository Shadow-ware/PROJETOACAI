using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace acaigalatico.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } // Ex: "Coca Cola 2L", "Luva M", "Granulado"

        [StringLength(200)]
        public string Descricao { get; set; } // Ex: "Fardo com 12 unidades"

        // Importante para calcular lucro: Quanto você pagou?
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoCusto { get; set; }

        // Por quanto você vende? (Se for insumo de uso interno, pode ser 0 ou null)
        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecoVenda { get; set; }

        public int EstoqueAtual { get; set; }
        public int EstoqueMinimo { get; set; } // Para o sistema avisar quando comprar mais

        public bool Ativo { get; set; } = true;

        // Define se aparece no cardápio ou se é só controle interno (ex: Touca, Luva)
        public bool EhParaVenda { get; set; }

        // Caminho da imagem do produto (ex: /images/produtos/acai-500.jpg)
        [StringLength(255)]
        public string ImagemUrl { get; set; }

        // Define se o produto aparece na seção de Novidades/Destaques
        public bool EmDestaque { get; set; }

        // Relacionamentos
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
