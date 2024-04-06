using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tarumt.CC.Ecommerce.Core.Services;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Services;

namespace Tarumt.CC.Ecommerce.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceConfig(this IServiceCollection services)
        {
            services.AddScoped<ServerSettingService>();
            services.AddScoped<UserService>();
            services.AddScoped<UserFileService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductCategoryService>();
            services.AddScoped<ProductCartService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        }
    }
}
