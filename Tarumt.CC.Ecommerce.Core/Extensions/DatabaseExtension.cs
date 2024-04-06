using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Infrastructure.Context;

namespace Tarumt.CC.Ecommerce.Extensions
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
                        string mySqlConnectionString = config.GetConnectionString("MySqlConnection")!;

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
