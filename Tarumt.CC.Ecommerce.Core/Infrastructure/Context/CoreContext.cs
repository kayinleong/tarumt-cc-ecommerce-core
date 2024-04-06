using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Infrastructure.Context;

public class CoreContext : DbContext
{
    public DbSet<ServerSetting> ServerSettings { get; set; }

    public DbSet<UserServerSetting> UserServerSettings { get; set; }

    public DbSet<UserPortalServerSetting> UserPortalServerSettings { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserMfa> UserMfas { get; set; }

    public DbSet<UserFile> UserFiles { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<ProductCart> ProductCarts { get; set; }

    public CoreContext(DbContextOptions options) : base(options)
    {
    }
}