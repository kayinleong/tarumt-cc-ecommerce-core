using Microsoft.EntityFrameworkCore;

namespace Tarumt.CC.Ecommerce.Infrastructure.Context
{
    public class CoreMySqlContext : CoreContext
    {
        public CoreMySqlContext(DbContextOptions options) : base(options)
        {
        }
    }
}
