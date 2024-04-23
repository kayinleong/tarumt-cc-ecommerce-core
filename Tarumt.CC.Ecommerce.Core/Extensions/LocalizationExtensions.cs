using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Tarumt.CC.Ecommerce.Core.Extensions
{
    public static class LocalizationExtensions
    {
        public static void AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                CultureInfo[] supportedCultures = new[]
                {
                    new CultureInfo("en-GB"),
                    new CultureInfo("zh-CN"),
                    new CultureInfo("my"),
                };

                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

    }
}
