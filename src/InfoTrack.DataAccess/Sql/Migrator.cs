using InfoTrack.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfoTrack.DataAccess.Sql
{
    internal class Migrator : IHostedService
    {
        private readonly ILogger<Migrator> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Migrator(
            ILogger<Migrator> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync(cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Migrator failed, apllication will shutdown.");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
