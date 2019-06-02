using FoodDelivery.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DAL.EntityFramework
{
    public class FoodDeliveryContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses {get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AdditionalInfoAboutMenuItem> AdditionalInfoAboutMenuItems { get; set; }

        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasketItem>()
            .HasKey(t => new { t.BasketId, t.MenuItemId });

            modelBuilder.Entity<BasketItem>()
                .HasOne(sc => sc.Basket)
                .WithMany(s => s.MenuItems)
                .HasForeignKey(sc => sc.BasketId);

            modelBuilder.Entity<BasketItem>()
                .HasOne(sc => sc.MenuItem)
                .WithMany(c => c.Baskets)
                .HasForeignKey(sc => sc.MenuItemId);
        }
    }
}