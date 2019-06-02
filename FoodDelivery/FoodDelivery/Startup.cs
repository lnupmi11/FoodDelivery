using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Models;
using System.IO;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.UnitOfWork;

namespace FoodDelivery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FoodDeliveryContext>(options =>
                options.UseSqlServer(connection, b => b.MigrationsAssembly("FoodDelivery")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<FoodDeliveryContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped(typeof(IUnitOfWork), typeof(FoodDeliveryUnitOfWork));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IBasketService), typeof(BasketService));
            services.AddScoped(typeof(IMenuService), typeof(MenuService));
            services.AddScoped(typeof(IPurchaseService), typeof(PurchaseService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IDiscountService), typeof(DiscountService));

            CreateRoles(services.BuildServiceProvider()).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        { 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = Enum.GetNames(typeof(Roles));

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                { 
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser user = await UserManager.FindByEmailAsync("admin@admin.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                };
                await UserManager.CreateAsync(user, "Admin@1");
            }
            await UserManager.AddToRoleAsync(user, Roles.Admin.ToString());

            ApplicationUser user1 = await UserManager.FindByEmailAsync("user@user.com");

            if (user1 == null)
            {
                user1 = new ApplicationUser()
                {
                    UserName = "user@user.com",
                    Email = "user@user.com",
                };
                await UserManager.CreateAsync(user1, "User@1");
            }
            await UserManager.AddToRoleAsync(user1, Roles.User.ToString());

            ApplicationUser user2 = await UserManager.FindByEmailAsync("manager@manager.com");

            if (user2 == null)
            {
                user2 = new ApplicationUser()
                {
                    UserName = "manager@manager.com",
                    Email = "manager@manager.com",
                };
                await UserManager.CreateAsync(user2, "Manager@1");
            }
            await UserManager.AddToRoleAsync(user2, Roles.OrderManager.ToString());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
