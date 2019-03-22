using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DAL.EntityFramework
{
    public class FoodDeliveryContext : DbContext
    {
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> ItemCategories { get; set; }

        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options)

            : base(options)
        {
            Database.EnsureCreated();
        }

        public FoodDeliveryContext()
            : base()
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category { Id =1, CategoryName = "Category1", Description = "Description1" },
                new Category { Id = 2, CategoryName = "Category2", Description = "Description2" });
            modelBuilder.Entity<Discount>().HasData(new Discount { Id = 1, Percentage = 0.05, Description = "Description1" },
                new Discount { Id = 2, Percentage = 0.1, Description = "Descripiton2" });
            modelBuilder.Entity<MenuItem>().HasData(new MenuItem { Id = 1, Name = "Item1", Description = "Description1", CategoryId = 1 },
                new MenuItem { Id = 2, Name = "Item2", Description = "Description2", CategoryId = 1 },
                new MenuItem { Id = 3, Name = "Item3", Description = "Description3", CategoryId = 1 , DiscountId = 2},
                new MenuItem { Id = 4, Name = "Item4", Description = "Description4", CategoryId = 1 , DiscountId = 1},
                new MenuItem { Id = 5, Name = "Item5", Description = "Description5", CategoryId = 1 });
        }
    }
}