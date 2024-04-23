using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Context;

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

    public DbSet<UserCard> UserCards {  get; set; }

    public DbSet<UserCart> UserCarts { get; set; }

    public DbSet<UserCartItem> UserCartItems { get; set; }

    public DbSet<UserOrder> UserOrders { get; set; }

    public CoreContext(DbContextOptions options) : base(options)
    {
    }
}