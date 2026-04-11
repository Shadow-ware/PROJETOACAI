using System;
using System.Threading;
using System.Threading.Tasks;
using acaigalatico.Infrastructure;
using acaigalatico.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Web.Services
{
    public class StartupSeedHostedService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<StartupSeedHostedService> _logger;

        public StartupSeedHostedService(IServiceProvider services, ILogger<StartupSeedHostedService> logger)
        {
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await db.Database.EnsureCreatedAsync(cancellationToken);

                    var seeding = scope.ServiceProvider.GetRequiredService<SeedingService>();
                    await seeding.SeedAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inicializar banco/seed.");
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
