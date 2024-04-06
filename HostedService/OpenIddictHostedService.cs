using OpenIddict.Abstractions;
using Tarumt.CC.Ecommerce.Infrastructure.Context;

namespace Tarumt.CC.Ecommerce.HostedService
{
    public class OpenIddictHostedService : IHostedService
    {
        private readonly IServiceProvider _serverProvider;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public OpenIddictHostedService(IServiceProvider serverProvider, IHostEnvironment environment, IConfiguration configuration)
        {
            _serverProvider = serverProvider;
            _environment = environment;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serverProvider.CreateScope();

            CoreContext context = scope.ServiceProvider.GetRequiredService<CoreContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            IOpenIddictApplicationManager applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            IEnumerable<IConfigurationSection> openIdClients = _configuration.GetSection("OpenIDClient:Applications").GetChildren();
            foreach (IConfigurationSection openIdClient in openIdClients)
            {
                switch (openIdClient.GetValue<string>("ClientType"))
                {
                    case "Private":
                        if (await applicationManager.FindByClientIdAsync(openIdClient.GetValue<string>("ClientName"), cancellationToken) == null)
                        {
                            OpenIddictApplicationDescriptor descriptor = new()
                            {
                                ClientId = openIdClient.GetValue<string>("ClientName"),
                                ClientSecret = openIdClient.GetValue<string>("ClientSecret"),
                                DisplayName = openIdClient.GetValue<string>("ClientDisplayName"),
                                RedirectUris = { new Uri(openIdClient.GetValue<string>("ClientRedirectUri")) },
                                Permissions =
                                {
                                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                                    OpenIddictConstants.Permissions.Endpoints.Token,
                                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                                    OpenIddictConstants.Permissions.Endpoints.Logout,

                                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                                    OpenIddictConstants.Permissions.Scopes.Profile,
                                    OpenIddictConstants.Permissions.Scopes.Address,
                                    OpenIddictConstants.Permissions.Scopes.Email,
                                    OpenIddictConstants.Permissions.Scopes.Phone,
                                    OpenIddictConstants.Permissions.Scopes.Roles,

                                    OpenIddictConstants.Permissions.ResponseTypes.Code
                                }
                            };

                            if (!string.IsNullOrEmpty(openIdClient.GetValue<string>("ClientPostLogoutRedirectUri")))
                            {
                                descriptor.PostLogoutRedirectUris.Add(new Uri(openIdClient.GetValue<string>("ClientPostLogoutRedirectUri")));
                            }

                            KeyValuePair<string, string>[] scopes = openIdClient.GetSection("Scopes").AsEnumerable().ToArray();
                            if (scopes.Length > 0)
                            {
                                foreach (KeyValuePair<string, string> scopeName in scopes)
                                {
                                    if (!string.IsNullOrEmpty(scopeName.Value))
                                    {
                                        descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scopeName.Value);
                                    }
                                }
                            }

                            await applicationManager.CreateAsync(descriptor, cancellationToken);
                        }
                        break;

                    case "Public":
                    default:
                        if (await applicationManager.FindByClientIdAsync(openIdClient.GetValue<string>("ClientName"), cancellationToken) == null)
                        {
                            OpenIddictApplicationDescriptor descriptor = new()
                            {
                                ClientId = openIdClient.GetValue<string>("ClientName"),
                                DisplayName = openIdClient.GetValue<string>("ClientDisplayName"),
                                RedirectUris = { new Uri(openIdClient.GetValue<string>("ClientRedirectUri")) },
                                Permissions =
                                {
                                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                                    OpenIddictConstants.Permissions.Endpoints.Token,
                                    OpenIddictConstants.Permissions.Endpoints.Introspection,
                                    OpenIddictConstants.Permissions.Endpoints.Logout,

                                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                                    OpenIddictConstants.Permissions.Scopes.Profile,
                                    OpenIddictConstants.Permissions.Scopes.Address,
                                    OpenIddictConstants.Permissions.Scopes.Email,
                                    OpenIddictConstants.Permissions.Scopes.Phone,
                                    OpenIddictConstants.Permissions.Scopes.Roles,

                                    OpenIddictConstants.Permissions.ResponseTypes.Code
                                }
                            };

                            if (!string.IsNullOrEmpty(openIdClient.GetValue<string>("ClientPostLogoutRedirectUri")))
                            {
                                descriptor.PostLogoutRedirectUris.Add(new Uri(openIdClient.GetValue<string>("ClientPostLogoutRedirectUri")));
                            }

                            KeyValuePair<string, string>[] scopes = openIdClient.GetSection("Scopes").AsEnumerable().ToArray();
                            if (scopes.Length > 0)
                            {
                                foreach (KeyValuePair<string, string> scopeName in scopes)
                                {
                                    if (!string.IsNullOrEmpty(scopeName.Value))
                                    {
                                        descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scopeName.Value);
                                    }
                                }
                            }

                            await applicationManager.CreateAsync(descriptor, cancellationToken);
                        }
                        break;
                }

            }

            IOpenIddictScopeManager scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            IEnumerable<IConfigurationSection> openIdResouces = _configuration.GetSection("OpenIDClient:Resources").GetChildren();
            foreach (IConfigurationSection openIdResource in openIdResouces)
            {
                if (await scopeManager.FindByNameAsync(openIdResource.GetValue<string>("Name"), cancellationToken) == null)
                {
                    OpenIddictScopeDescriptor descriptor = new()
                    {
                        Name = openIdResource.GetValue<string>("Name"),
                        DisplayName = openIdResource.GetValue<string>("DisplayName")
                    };

                    KeyValuePair<string, string>[] resources = openIdResource.GetSection("Resources").AsEnumerable().ToArray();
                    if (resources.Length > 0)
                    {
                        foreach (KeyValuePair<string, string> resourceName in resources)
                        {
                            if (!string.IsNullOrEmpty(resourceName.Value))
                            {
                                descriptor.Resources.Add(resourceName.Value);
                            }
                        }
                    }

                    await scopeManager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
