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
        public List<ItemPedidoDto> Itens { get; set; } = new();
    }

    public sealed class ItemPedidoDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public ProdutoDto? Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal => Quantidade * PrecoUnitario;

        public string DisplayNome => NomeProduto ?? Produto?.Nome ?? "Produto";
    }

    public sealed class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
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
