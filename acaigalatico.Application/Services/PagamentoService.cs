using System;
using System.Threading.Tasks;
using acaigalatico.Application.Interfaces;

namespace acaigalatico.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        // Simulação de Processamento Real (Estrutura para futura API via HttpClient)
        public async Task<(bool Sucesso, string TransacaoId, string Mensagem)> ProcessarPagamentoCartaoAsync(string nome, string numero, string validade, string cvv, decimal valor)
        {
            // Simular delay de processamento de gateway (Stripe/Mercado Pago)
            await Task.Delay(1500);

            // Validação simples simulada (Ex: cartões terminados em 0000 falham)
            if (numero.EndsWith("0000"))
            {
                return (false, "", "O pagamento foi recusado pela operadora do cartão.");
            }

            // Simular sucesso e retorno de ID de transação real
            string transacaoId = $"TX-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            return (true, transacaoId, "Pagamento aprovado com sucesso!");
        }

        public async Task<(bool Sucesso, string PixCopiaECola, string Mensagem)> GerarPixAsync(decimal valor)
        {
            await Task.Delay(800);
            string pixChave = $"00020126360014BR.GOV.BCB.PIX0114acaigalatico{valor:F2}5204000053039865802BR5915AcaiGalatico6009SaoPaulo62070503***6304";
            return (true, pixChave, "Chave Pix gerada com sucesso!");
        }
    }
}
