using System.Threading.Tasks;

namespace acaigalatico.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<(bool Sucesso, string TransacaoId, string Mensagem)> ProcessarPagamentoCartaoAsync(string nome, string numero, string validade, string cvv, decimal valor);
        Task<(bool Sucesso, string PixCopiaECola, string Mensagem)> GerarPixAsync(decimal valor);
    }
}
