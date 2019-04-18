using NUnit.Framework;
using FoodDelivery.DAL.Interfaces;
using Moq;
using FoodDelivery.DAL.Models;
using FoodDelivery.BLL.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using FoodDelivery.DTO.Menu;

namespace FoodDelivery.TEST
{
    class MenuServiceTest
    {
        IUnitOfWork foodDeliveryUnitOfWork;
        [SetUp]
        public void Setup()
        {
            var userRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            userRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetUserRepositoryQuery());

            var menuItemRepositoryMock = new Mock<IRepository<MenuItem>>();
            menuItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetMenuItemRepositoryQuery());
            menuItemRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string menuItemId) => GetMenuItem(menuItemId));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.UsersRepository).Returns(userRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.MenuItemsRepository).Returns(menuItemRepositoryMock.Object);

            foodDeliveryUnitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void AddNewMenuItemSuccssesfully()
        {
            string menuItemId = "MenuItemId";
            var menu = foodDeliveryUnitOfWork.MenuItemsRepository.GetQuery();

            var menuItem = menu.FirstOrDefault(mi => mi.Id == menuItemId);
            Assert.AreEqual(null, menuItem);

            MenuService menuService = new MenuService(foodDeliveryUnitOfWork);
            menuService.Add(new MenuItemDTO() { Id = menuItemId });

            menu = foodDeliveryUnitOfWork.MenuItemsRepository.GetQuery();
            menuItem = menu.FirstOrDefault(mi => mi.Id == menuItemId);

            var epectedResult = foodDeliveryUnitOfWork.MenuItemsRepository.Get(menuItemId);

            Assert.AreEqual(epectedResult.Id, menuItem.Id);
        }

        public IQueryable<ApplicationUser> GetUserRepositoryQuery()
        {
            var menuItems = GetMenuItems();
            var basketItems = GetBasketItems(menuItems);
            var baskets = GetBaskets(basketItems);
            return GetUsers(baskets).AsQueryable();
        }

        public IQueryable<MenuItem> GetMenuItemRepositoryQuery()
        {
            return GetMenuItems().AsQueryable();
        }

        public MenuItem GetMenuItem(string itemId)
        {
            return GetMenuItems().FirstOrDefault(i => i.Id == itemId);
        }

        public List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100 },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200 },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300 },
                new MenuItem { Id = "fourthMenuItemId", Name = "fourthMenuItemName", Description = "fourthMenuItemDescription", Price = 400 },
                new MenuItem { Id = "fifthMenuItemId", Name = "fifthMenuItemName", Description = "fifthMenuItemDescription", Price = 500 }
            };
        }

        public List<BasketItem> GetBasketItems(List<MenuItem> menuItems)
        {
            return new List<BasketItem>
            {
                new BasketItem { Count = 1, MenuItem = menuItems[0], MenuItemId = menuItems[0].Id },
                new BasketItem { Count = 2, MenuItem = menuItems[1], MenuItemId = menuItems[1].Id },
                new BasketItem { Count = 2, MenuItem = menuItems[2], MenuItemId = menuItems[2].Id },
                new BasketItem { Count = 3, MenuItem = menuItems[3], MenuItemId = menuItems[3].Id },
                new BasketItem { Count = 3, MenuItem = menuItems[4], MenuItemId = menuItems[4].Id },
                new BasketItem { Count = 2, MenuItem = menuItems[1], MenuItemId = menuItems[1].Id },
                new BasketItem { Count = 2, MenuItem = menuItems[2], MenuItemId = menuItems[2].Id }
            };
        }

        public List<Basket> GetBaskets(List<BasketItem> basketItems)
        {
            return new List<Basket>
            {
                new Basket { Id = "firstBasket", MenuItems = new List<BasketItem> { basketItems[0], basketItems[1], basketItems[2] } },
                new Basket { Id = "secondBasket", MenuItems = new List<BasketItem> { basketItems[3], basketItems[4], basketItems[5] } }
            };
        }

        public List<ApplicationUser> GetUsers(List<Basket> baskets)
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser{ UserName = "firstTestUser", Basket = baskets[0]},
                new ApplicationUser{ UserName = "secondTestUser", Basket = baskets[1]}
            };
        }
    }
}
