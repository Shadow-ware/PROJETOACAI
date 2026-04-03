using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } // Ex: "Bebidas", "Acompanhamentos", "Uso Interno", "Salgadinhos"

        public ICollection<Produto> Produtos { get; set; }
    }
}
