using Microsoft.IdentityModel.Tokens;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Tarumt.CC.Ecommerce.Extensions
{
    public static class OpenIddictExtensions
    {
        public static void AddOpenIddictConfig(this IServiceCollection services, IWebHostEnvironment env, ConfigurationManager config)
        {
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<CoreContext>();
                })
                .AddServer(options =>
                {
                    if (env.IsDevelopment())
                    {
                        options.AddEncryptionKey(new SymmetricSecurityKey(
                            Convert.FromBase64String("ZjNiMjM4N2UtOThhMS00OWUxLWI2YTItOGMwZTE3NGI=")));
                        options.AddDevelopmentSigningCertificate();
                    }
                    else
                    {
                        options.AddEncryptionKey(new SymmetricSecurityKey(
                            Convert.FromBase64String("ZjNiMjM4N2UtOThhMS00OWUxLWI2YTItOGMwZTE3NGI=")));
                        options.AddDevelopmentSigningCertificate();
                    }

                    bool accessTokenLifetimeSuccess = int.TryParse(config["OpenIDServer:ServerAccessTokenLifetime"], out int accessTokenLifetime);
                    if (!accessTokenLifetimeSuccess)
                    {
                        throw new InvalidOperationException();
                    }

                    bool refreshTokenLifetimeSuccess = int.TryParse(config["OpenIDServer:ServerRefreshTokenLifetime"], out int refreshTokenLifetime);
                    if (!refreshTokenLifetimeSuccess)
                    {
                        throw new InvalidOperationException();
                    }

                    options.SetAccessTokenLifetime(TimeSpan.FromMilliseconds(accessTokenLifetime))
                        .SetRefreshTokenLifetime(TimeSpan.FromMilliseconds(refreshTokenLifetime));

                    options.AllowClientCredentialsFlow()
                        .AllowAuthorizationCodeFlow()
                        .AllowRefreshTokenFlow()
                        .RequireProofKeyForCodeExchange();

                    options.SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetIntrospectionEndpointUris("/connect/introspect")
                        .SetUserinfoEndpointUris("/connect/userinfo")
                        .SetVerificationEndpointUris("/connect/verify")
                        .SetLogoutEndpointUris("/connect/logout");

                    options.RegisterScopes(
                        Scopes.Profile,
                        Scopes.Email,
                        Scopes.Roles,
                        Scopes.Phone,
                        Scopes.Address,
                        Scopes.Roles);

                    options.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableStatusCodePagesIntegration();
                })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.AddAudiences("ky_web_cms_core");
            });
        }
    }
}
