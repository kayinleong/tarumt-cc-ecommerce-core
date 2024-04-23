using Microsoft.AspNetCore.Identity;
using Tarumt.CC.Ecommerce.Core.Constants;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Core.HostedService
{
    public class UserHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public UserHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            CoreContext context = scope.ServiceProvider.GetRequiredService<CoreContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            if (context.Users.Any())
            {
                return;
            }

            IPasswordHasher<User> passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            User adminUser = new()
            {
                Username = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                DateOfBirth = DateTime.Now,
                Gender = GenderTypes.MALE,
                Email = "admin@web.com",
                PhoneNumber = "0123456789",
                Address = "Admin Address",
                Culture = "en-GB",
                UserMfa = new UserMfa(),
                IsAdmin = true,
            };

            adminUser.PasswordHashed = passwordHasher.HashPassword(adminUser, "abc123456789");

            await context.Users.AddAsync(adminUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
