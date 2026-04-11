namespace acaiGalatico.UI.Models
{
    public sealed class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public decimal SaldoDevedor { get; set; }
    }
}
