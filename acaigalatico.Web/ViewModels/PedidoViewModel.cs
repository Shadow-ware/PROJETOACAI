using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace acaigalatico.Web.ViewModels
{
    public class PedidoViewModel
    {
        // Dados do Pedido
        public string? Type { get; set; }

        public string? Size { get; set; }

        public int Quantity { get; set; } = 1;

        public List<string>? Fruits { get; set; }
        public List<string>? Toppings { get; set; }

        // Dados de Pagamento (NÃO PERSISTIDOS)
        public string? PaymentMethod { get; set; } = "card";

        [Display(Name = "Nome no Cartão")]
        public string? CardName { get; set; }

        [Display(Name = "Número do Cartão")]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "Número de cartão inválido")]
        public string? CardNumber { get; set; }

        [Display(Name = "Validade")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Formato MM/AA")]
        public string? CardExpiry { get; set; }

        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV inválido")]
        public string? CardCvv { get; set; }
    }
}
