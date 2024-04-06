using Microsoft.AspNetCore.Authentication.Cookies;

namespace Tarumt.CC.Ecommerce.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    bool cookieLifetimeSuccess = int.TryParse(config["Cookie:CookieLifetime"], out int cookieLifetime);
                    if (!cookieLifetimeSuccess)
                    {
                        throw new InvalidOperationException();
                    }

                    options.ExpireTimeSpan = TimeSpan.FromMilliseconds(cookieLifetime);
                    options.Cookie.MaxAge = options.ExpireTimeSpan;
                    options.Cookie.Name = "token";
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                    options.ReturnUrlParameter = "returnUrl";
                });
        }
    }
}
