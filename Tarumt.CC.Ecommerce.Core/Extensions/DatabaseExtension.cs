using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;

namespace Tarumt.CC.Ecommerce.Core.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddDatabasesConfig(this IServiceCollection services, ConfigurationManager config)
        {
            switch (config["DatabaseProvider"])
            {
                case "MySql":
                    services.AddDbContextPool<CoreContext, CoreMySqlContext>(options =>
                    {
                        string mySqlConnectionString = $"Server=${Environment.GetEnvironmentVariable("DB_HOST")};Port=3306;Database=Dev_Tarumt_CC_Ecommerce_Core;User=ky;Password=${Environment.GetEnvironmentVariable("DB_KEY")}";

                        options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString));
                        options.UseOpenIddict();
                    });
                    break;

                case "SqlServer":
                    services.AddDbContextPool<CoreContext, CoreSqlServerContext>(options =>
                    {
                        string sqlServerConnectionString = config.GetConnectionString("SqlServerConnection")!;

                        options.UseSqlServer(sqlServerConnectionString);
                        options.UseOpenIddict();
                    });
                    break;
            }
        }
    }
}
