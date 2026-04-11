namespace acaiGalatico.UI.Models
{
    public sealed class PedidoDto
    {
        public int Id { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }
        public TipoPagamentoDto FormaPagamento { get; set; } = TipoPagamentoDto.Dinheiro;
        public int? ClienteId { get; set; }
        public ClienteDto? Cliente { get; set; }
        public StatusVendaDto Status { get; set; } = StatusVendaDto.Pendente;
        public string EnderecoEntrega { get; set; } = string.Empty;
        public string BairroEntrega { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
    }

    public enum TipoPagamentoDto
    {
        Dinheiro,
        Pix,
        Cartao,
        Fiado
    }

    public enum StatusVendaDto
    {
        Pendente,
        Preparando,
        SaiuParaEntrega,
        Entregue,
        Cancelado
    }
}
