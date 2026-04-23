using System.Text.RegularExpressions;

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

        // Propriedades parseadas da Observacao (Regex mais flexível)
        private string GetValue(string key)
        {
            if (string.IsNullOrEmpty(Observacao)) return string.Empty;
            // Busca a chave e pega o conteúdo até o próximo | ou || ou fim da string
            var match = Regex.Match(Observacao, $@"{key}:\s*(.*?)(?:\||\|\||$)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        public string TipoParsed => GetValue("Tipo");
        public string TamanhoParsed => GetValue("Tamanho");
        public string QuantidadeParsed => GetValue("Quantidade");
        public string FrutasParsed => GetValue("Frutas");
        public string AcompanhamentosParsed => GetValue("Acompanhamentos");
        public string PagamentoParsed => GetValue("Pagamento");
        public string ClienteParsed => GetValue("Cliente");

        // Propriedades de auxílio para exibição
        public string ClienteNomeDisplay 
        {
            get 
            {
                if (!string.IsNullOrEmpty(Cliente?.Nome)) return Cliente.Nome;
                
                // Tenta extrair o nome do cliente da string de observação (ex: "Cliente: João Silva (email@...) | ...")
                var match = Regex.Match(Observacao, @"Cliente:\s*(.*?)(?:\s*\(|$|\|)", RegexOptions.IgnoreCase);
                if (match.Success && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
                {
                    return match.Groups[1].Value.Trim();
                }

                return "Cliente Avulso";
            }
        }
        
        public string PagamentoLimpo 
        {
            get 
            {
                string pg = !string.IsNullOrEmpty(PagamentoParsed) ? PagamentoParsed : FormaPagamento.ToString();
                // Remove textos de aguardando ou detalhes entre parênteses para ficar limpo
                pg = Regex.Replace(pg, @"\(.*?\)", "").Trim();
                return pg;
            }
        }
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
