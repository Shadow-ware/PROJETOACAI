using System.Net.Http.Json;
using System.Text.Json;
using acaiGalatico.UI.Models;

namespace acaiGalatico.UI.Services
{
    public sealed class PedidosApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PedidosApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string BaseUrl => _httpClient.BaseAddress?.ToString() ?? string.Empty;

        public async Task<List<PedidoDto>> GetPedidosAsync(CancellationToken cancellationToken = default)
        {
            try 
            {
                // TESTE DE CONEXÃO REAL
                var urlCompleta = _httpClient.BaseAddress + "api/pedidos";
                Console.WriteLine($"[HTTP-DEBUG] Chamando API: {urlCompleta}");
                
                var response = await _httpClient.GetAsync("api/pedidos", cancellationToken);
                
                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API retornou erro {response.StatusCode}: {erro}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<PedidoDto>>(json, _jsonOptions) ?? new List<PedidoDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"FALHA CRÍTICA NO DESKTOP:\nNão foi possível ler dados de http://localhost:5207/api/pedidos\n\nDetalhe: {ex.Message}");
            }
        }

        public async Task<List<ClienteDto>> GetClientesAsync(CancellationToken cancellationToken = default)
        {
            var clientes = await _httpClient.GetFromJsonAsync<List<ClienteDto>>("api/clientes", _jsonOptions, cancellationToken);
            return clientes ?? new List<ClienteDto>();
        }

        public async Task<PedidoDto> CreatePedidoAsync(PedidoDto pedido, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/pedidos", pedido, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdPedido = await response.Content.ReadFromJsonAsync<PedidoDto>(_jsonOptions, cancellationToken);
            return createdPedido ?? throw new InvalidOperationException("A API não retornou o pedido criado.");
        }

        public async Task<PedidoDto> GetPedidoByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var pedido = await _httpClient.GetFromJsonAsync<PedidoDto>($"api/pedidos/{id}", _jsonOptions, cancellationToken);
            return pedido ?? throw new Exception("Pedido não encontrado");
        }

        public async Task UpdateStatusAsync(int id, StatusVendaDto status, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PatchAsJsonAsync($"api/pedidos/{id}/status", status, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePedidoAsync(PedidoDto pedido, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PutAsJsonAsync($"api/pedidos/{pedido.Id}", pedido, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePedidoAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.DeleteAsync($"api/pedidos/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
