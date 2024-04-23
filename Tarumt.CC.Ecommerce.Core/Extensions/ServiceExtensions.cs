using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Core.Services;

namespace Tarumt.CC.Ecommerce.Core.Extensions
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
            services.AddScoped<UserOrderService>();
            services.AddScoped<UserCardService>();
            services.AddScoped<UserCartService>();
            services.AddScoped<UserCartItemService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        }
    }
}
