using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace acaigalatico.Domain.Entities
{
    public class ItemVendaAdicional
    {
        public int Id { get; set; }

        public int ItemVendaId { get; set; }
        public ItemVenda ItemVenda { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } // O adicional (Ex: Granola, Leite em Pó)

        public int Quantidade { get; set; } // Geralmente 1

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; } // Preço cobrado pelo adicional
    }
}
