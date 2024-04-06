
using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Infrastructure.Context;

namespace Tarumt.CC.Ecommerce.HostedService
{
    public class ServerSettingHostedService : IHostedService
    {
        private readonly IServiceProvider _serverProvider;

        public ServerSettingHostedService(IServiceProvider serverProvider)
        {
            _serverProvider = serverProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serverProvider.CreateScope();

            CoreContext context = scope.ServiceProvider.GetRequiredService<CoreContext>();

            if (await context.ServerSettings.AnyAsync())
            {
                return;
            }

            await context.ServerSettings.AddAsync(new()
            {
                UserServerSettings = new()
                {
                    RequiredUserAddress = false,
                    RequiredStrongPasswordValidation = true
                },
                UserPortalServerSettings = new()
                {
                    EnableLoginPage = true,
                    EnableRegisterPage = true,
                    EnablePasswordForget = true
                }
            });
            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
