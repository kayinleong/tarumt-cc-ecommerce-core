using Microsoft.EntityFrameworkCore;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Context
{
    public class CoreMySqlContext : CoreContext
    {
        public CoreMySqlContext(DbContextOptions options) : base(options)
        {
        }
    }
}
