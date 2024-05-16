﻿namespace Tarumt.CC.Ecommerce.Core.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("PRODUCTION", policyOptions =>
                {
                    policyOptions.WithOrigins("https://ec2-3-237-236-97.compute-1.amazonaws.com", "https://ec2-3-233-238-170.compute-1.amazonaws.com")
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
