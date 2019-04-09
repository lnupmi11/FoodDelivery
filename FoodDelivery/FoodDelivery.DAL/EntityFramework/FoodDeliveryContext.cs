﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}