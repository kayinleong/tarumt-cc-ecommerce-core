using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Tarumt.CC.Ecommerce.Extensions
{
    public static class ResponseCompressionExtensions
    {
        public static void AddResponseCompressionConfig(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }
    }
}
