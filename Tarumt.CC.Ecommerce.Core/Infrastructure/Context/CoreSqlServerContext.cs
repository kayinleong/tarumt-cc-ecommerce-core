using Microsoft.EntityFrameworkCore;

namespace Tarumt.CC.Ecommerce.Infrastructure.Context
{
    public class CoreSqlServerContext : CoreContext
    {
        public CoreSqlServerContext(DbContextOptions options) : base(options)
        {
        }
    }
}
