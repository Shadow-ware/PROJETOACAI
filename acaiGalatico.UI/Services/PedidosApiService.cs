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
            var pedidos = await _httpClient.GetFromJsonAsync<List<PedidoDto>>("api/orders", _jsonOptions, cancellationToken);
            return pedidos ?? new List<PedidoDto>();
        }

        public async Task<List<ClienteDto>> GetClientesAsync(CancellationToken cancellationToken = default)
        {
            var clientes = await _httpClient.GetFromJsonAsync<List<ClienteDto>>("api/customers", _jsonOptions, cancellationToken);
            return clientes ?? new List<ClienteDto>();
        }

        public async Task<PedidoDto> CreatePedidoAsync(PedidoDto pedido, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/orders", pedido, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdPedido = await response.Content.ReadFromJsonAsync<PedidoDto>(_jsonOptions, cancellationToken);
            return createdPedido ?? throw new InvalidOperationException("A API não retornou o pedido criado.");
        }

        public async Task UpdatePedidoAsync(PedidoDto pedido, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.PutAsJsonAsync($"api/orders/{pedido.Id}", pedido, _jsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePedidoAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.DeleteAsync($"api/orders/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
