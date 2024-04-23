using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;

namespace Tarumt.CC.Ecommerce.Core.HostedService
{
    public class DatabaseHostedService : IHostedService
    {
        private readonly IServiceProvider _serverProvider;

        public DatabaseHostedService(IServiceProvider serverProvider)
        {
            _serverProvider = serverProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serverProvider.CreateScope();

            CoreContext context = scope.ServiceProvider.GetRequiredService<CoreContext>();
            await context.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
