namespace Tarumt.CC.Ecommerce.Core.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("PRODUCTION", policyOptions =>
                {
                    policyOptions.WithOrigins("http://localhost:8011", "http://localhost:8012")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                options.AddPolicy("DEVELOPMENT", policyOptions =>
                {
                    policyOptions.WithOrigins("http://localhost:4200", "http://localhost:4100")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void UseCorsConfig(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("DEVELOPMENT");
            }
            else
            {
                app.UseCors("PRODUCTION");
                app.UseResponseCompression();
            }
        }
    }
}
