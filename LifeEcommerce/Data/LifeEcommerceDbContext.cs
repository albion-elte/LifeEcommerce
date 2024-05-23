using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Data
{
    public class LifeEcommerceDbContext : DbContext
    {
        public LifeEcommerceDbContext(DbContextOptions<LifeEcommerceDbContext> options) : base(options) 
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<OrderData> OrdersData { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCard> ShoppingCards { get; set; }
    }
}
