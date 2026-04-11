using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations.Schema;

namespace acaigalatico.Domain.Entities
{
    public class ItemVenda
    {
        public int Id { get; set; }

        public int VendaId { get; set; }
        public Venda Venda { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public int Quantidade { get; set; }

        // Salvamos o preço aqui também para o histórico não mudar se o preço do produto subir depois
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; }

        public ICollection<ItemVendaAdicional> Adicionais { get; set; }
    }
}