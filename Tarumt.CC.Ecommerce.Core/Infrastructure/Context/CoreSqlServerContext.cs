using Microsoft.EntityFrameworkCore;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Context
{
    public class CoreSqlServerContext : CoreContext
    {
        public CoreSqlServerContext(DbContextOptions options) : base(options)
        {
        }
    }
}
