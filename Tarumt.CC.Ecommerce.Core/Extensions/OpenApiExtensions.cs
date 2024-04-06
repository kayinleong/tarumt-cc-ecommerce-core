using Microsoft.OpenApi.Models;

namespace Tarumt.CC.Ecommerce.Core.Extensions
{
    public static class OpenApiExtensions
    {
        public static void AddOpenApiConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("public_v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tarumt.CC.Ecommerce API",
                    Description = "TAR UMT Cloud Computing Assignment - Ecommerce Website",
                });

                options.SwaggerDoc("private_v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tarumt.CC.Ecommerce Admin API",
                    Description = "TAR UMT Cloud Computing Assignment - Ecommerce Website",
                });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName == "public_v1" && apiDesc.RelativePath!.StartsWith("api/") && !apiDesc.RelativePath!.Contains("admin/"))
                    {
                        return true;
                    }

                    if (docName == "private_v1" && apiDesc.RelativePath!.StartsWith("api/") && apiDesc.RelativePath!.Contains("admin/"))
                    {
                        return true;
                    }

                    return false;
                });
            });
        }

        public static void UseOpenApiConfig(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/public_v1/swagger.json", "public_v1");
                options.SwaggerEndpoint("/swagger/private_v1/swagger.json", "private_v1");
            });
        }
    }
}
