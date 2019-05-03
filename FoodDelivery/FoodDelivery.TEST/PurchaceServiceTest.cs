using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class PurchaceServiceTest
    {
        IUnitOfWork foodDeliveryUnitOfWork;

        [SetUp]
        public void Setup()
        {
            Orders = new List<Order>();
            var userRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            userRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetUserRepositoryQuery());

            var orderItemRepositoryMock = new Mock<IRepository<OrderItem>>();
            orderItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetOrderItemRepositoryQuery());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.UsersRepository).Returns(userRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.OrderItemsRepository).Returns(orderItemRepositoryMock.Object);
            foodDeliveryUnitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void GetUserPurchacesListSuccssesfully()
        {
            string userName = "firstTestUser";
            PurchaceService purchaceService = new PurchaceService(foodDeliveryUnitOfWork);
            var purchaceList = purchaceService.GetListOfPurchaces(userName);
            Assert.AreEqual(purchaceList.Count, 2);
        }

        [Test]
        public void GetUserPurchacesListNoUserWithSuchName()
        {
            string userName = "randomUser";
            PurchaceService purchaceService = new PurchaceService(foodDeliveryUnitOfWork);
            var ex = Assert.Throws<ArgumentException>(() => purchaceService.GetListOfPurchaces(userName));
            Assert.That(ex.Message, Is.EqualTo($"There is no user item with the following userName: { userName }"));
        }

        [Test]
        public void GetItemsFromPurchaceListSuccssesfully()
        {
            string orderId = "firstOrder";
            PurchaceService purchaceService = new PurchaceService(foodDeliveryUnitOfWork);
            var purchaceItemIds = purchaceService.GetPurchaceItems(orderId).Select(i=>i.Id).ToArray();
            string[] orderItemsIds = new string[] { "firstOrdertemId", "secondOrdertemId", "thirdOrdertemId" };
            Assert.AreEqual(orderItemsIds.Count(), orderItemsIds.Intersect(purchaceItemIds).Count());
        }

        [Test]
        public void GetItemsFromPurchaceListIncorrectPurchaceId()
        {
            string orderId = "randomOrder";
            PurchaceService purchaceService = new PurchaceService(foodDeliveryUnitOfWork);
            var purchaceItemIds = purchaceService.GetPurchaceItems(orderId).Select(i => i.Id).ToArray();
            Assert.IsTrue(!purchaceItemIds.Any());
        }

        public IQueryable<ApplicationUser> GetUserRepositoryQuery()
        {
            var menuItems = GetMenuItems();
            var orderItems = GetOrderItems(menuItems);
            var orders = GetOrders(orderItems);
            return GetUsers(orders).AsQueryable();
        }

        public IQueryable<OrderItem> GetOrderItemRepositoryQuery()
        {
            var menuItems = GetMenuItems();
            return GetOrderItems(menuItems).AsQueryable();
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

        public List<OrderItem> GetOrderItems(List<MenuItem> menuItems)
        {
            return new List<OrderItem>
            {
                new OrderItem { OrderItemId = "firstOrdertemId", Count = 1, MenuItem = menuItems[0], MenuItemId = menuItems[0].Id, Order = new Order { OrderId = "firstOrder" } },
                new OrderItem { OrderItemId = "secondOrdertemId", Count = 2, MenuItem = menuItems[1], MenuItemId = menuItems[1].Id, Order = new Order { OrderId = "firstOrder" } },
                new OrderItem { OrderItemId = "thirdOrdertemId", Count = 2, MenuItem = menuItems[2], MenuItemId = menuItems[2].Id, Order = new Order { OrderId = "firstOrder" } },
                new OrderItem { OrderItemId = "fourthOrdertemId", Count = 3, MenuItem = menuItems[3], MenuItemId = menuItems[3].Id, Order = new Order { OrderId = "secondOrder" } },
                new OrderItem { OrderItemId = "fifthOrdertemId", Count = 3, MenuItem = menuItems[4], MenuItemId = menuItems[4].Id, Order = new Order { OrderId = "secondOrder" } },
                new OrderItem { OrderItemId = "sixthOrdertemId", Count = 2, MenuItem = menuItems[1], MenuItemId = menuItems[1].Id, Order = new Order { OrderId = "secondOrder" } },
                new OrderItem { OrderItemId = "seventhOrdertemId", Count = 2, MenuItem = menuItems[2], MenuItemId = menuItems[2].Id, Order = new Order { OrderId = "secondOrder" } }
            };
        }

        public List<Order> GetOrders(List<OrderItem> orderItems)
        {
            return new List<Order>
            {
                new Order { OrderId = "firstOrder", OrderItems = new List<OrderItem> { orderItems[0], orderItems[1], orderItems[2] } },
                new Order { OrderId = "secondOrder", OrderItems = new List<OrderItem> { orderItems[3], orderItems[4], orderItems[5] } }
            };
        }

        public List<ApplicationUser> GetUsers(List<Order> orders)
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser{ UserName = "firstTestUser", Orders = new List<Order> {orders[0], orders[1] } },
                new ApplicationUser{ UserName = "secondTestUser", Orders = new List<Order> { orders[0], orders[1] } }
            };
        }

        public List<Order> Orders { get; set; }
    }
}
