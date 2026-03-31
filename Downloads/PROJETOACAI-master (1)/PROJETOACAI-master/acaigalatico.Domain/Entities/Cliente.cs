using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace acaigalatico.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; } // Chave Pix ou WhatsApp

        // Saldo Devedor (Quanto ele deve na casa)
        [Column(TypeName = "decimal(10,2)")]
        public decimal SaldoDevedor { get; set; }

        // Histórico de compras (Vendas)
        public ICollection<Venda> Compras { get; set; }
    }
}
