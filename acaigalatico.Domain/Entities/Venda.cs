using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Domain.Entities
{
    public class Venda
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }

        // Forma de Pagamento: Dinheiro, Pix, Cartão, Fiado
        public TipoPagamento FormaPagamento { get; set; }

        public int? ClienteId { get; set; } // Pode ser nulo se for venda avulsa anonima
        public Cliente? Cliente { get; set; }

        // Informações de Entrega
        public StatusVenda Status { get; set; } = StatusVenda.Pendente;
        
        [StringLength(200)]
        public string? EnderecoEntrega { get; set; } // Rua, Número, Complemento
        
        [StringLength(100)]
        public string? BairroEntrega { get; set; }
        
        [StringLength(500)]
        public string? Observacao { get; set; } // Ex: "Sem campainha", "Troco para 50"

        public ICollection<ItemVenda>? Itens { get; set; }
    }

    public enum TipoPagamento { Dinheiro, Pix, Cartao, Fiado }

    public enum StatusVenda
    {
        Pendente,       // Recebido
        Preparando,     // Em produção
        SaiuParaEntrega,// Entregador saiu
        Entregue,       // Finalizado
        Cancelado       // Cancelado
    }
}