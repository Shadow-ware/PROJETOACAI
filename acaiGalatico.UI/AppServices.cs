using acaiGalatico.UI.Services;

namespace acaiGalatico.UI
{
    internal static class AppServices
    {
        private static readonly Lazy<PedidosApiService> _pedidosApiService = new(CreatePedidosApiService);

        public static PedidosApiService PedidosApiService => _pedidosApiService.Value;

        private static PedidosApiService CreatePedidosApiService()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5207/"),
                Timeout = TimeSpan.FromSeconds(30)
            };

            return new PedidosApiService(httpClient);
        }
    }
}
