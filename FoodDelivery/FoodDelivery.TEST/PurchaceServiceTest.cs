using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class PurchaseServiceTest
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
        public void GetUserPurchasesListSuccssesfully()
        {
            string userName = "firstTestUser";
            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var purchaseList = purchaseService.GetListOfPurchases(userName);
            Assert.AreEqual(purchaseList.Count, 2);
        }

        [Test]
        public void GetUserPurchasesListNoUserWithSuchName()
        {
            string userName = "randomUser";
            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var ex = Assert.Throws<ArgumentException>(() => purchaseService.GetListOfPurchases(userName));
            Assert.That(ex.Message, Is.EqualTo($"There is no user item with the following userName: { userName }"));
        }

        [Test]
        public void GetItemsFromPurchaseListSuccssesfully()
        {
            string orderId = "firstOrder";
            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var purchaseItemIds = purchaseService.GetPurchaseItems(orderId).Select(i=>i.Id).ToArray();
            string[] orderItemsIds = new string[] { "firstOrdertemId", "secondOrdertemId", "thirdOrdertemId" };
            Assert.AreEqual(orderItemsIds.Count(), orderItemsIds.Intersect(purchaseItemIds).Count());
        }

        [Test]
        public void GetItemsFromPurchaseListIncorrectPurchaseId()
        {
            string orderId = "randomOrder";
            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var purchaseItemIds = purchaseService.GetPurchaseItems(orderId).Select(i => i.Id).ToArray();
            Assert.IsTrue(!purchaseItemIds.Any());
        }

        [Test]
        public void GetItemsFromUserCartByPriceDescendingSuccessfully()
        {
            string orderId = "firstOrder";

            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var filterObj = new FilterMenuItem
            {
                CategoryId = string.Empty,
                FilterOpt = "desc",
                ItemPerPage = 3,
                Page = 1,
                SearchWord = string.Empty
            };
            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId);
            var resultIds = result.Select(r => r.Id).ToArray();
            var resultOrderedByDescIds = result.OrderByDescending(r => r.Price).Select(r => r.Id).ToArray();

            bool isValidResult = true;
            for (int i = 0; i < filterObj.ItemPerPage; i++)
            {
                isValidResult = resultIds[i] == resultOrderedByDescIds[i];
            }

            Assert.IsTrue(isValidResult);
        }

        [Test]
        public void GetItemsFromUserCartByItemNameSuccessfully()
        {
            string orderId = "firstOrder";
            const string searchedItemName = "firstMenuItemId";

            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var filterObj = new FilterMenuItem
            {
                CategoryId = string.Empty,
                FilterOpt = "desc",
                ItemPerPage = 3,
                Page = 1,
                SearchWord = searchedItemName
            };
            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId);
            Assert.IsTrue(result.All(r => r.Name.Contains(searchedItemName)));
        }

        [Test]
        public void GetItemsFromUserCartByCategorySuccessfully()
        {
            string orderId = "firstOrder";

            const string searchedCtegoryId = "firstCategoryId";
            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var filterObj = new FilterMenuItem
            {
                CategoryId = searchedCtegoryId,
                FilterOpt = string.Empty,
                ItemPerPage = 3,
                Page = 1,
                SearchWord = string.Empty
            };

            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId);
            Assert.IsTrue(result.All(r => r.Category.Id == searchedCtegoryId));
        }

        [Test]
        public void GetItemsFromUserCartByNotExistingCategory()
        {
            const string searchedCtegoryId = "randomCategoryId";
            string orderId = "firstOrder";

            var filterObj = new FilterMenuItem
            {
                CategoryId = searchedCtegoryId,
                FilterOpt = string.Empty,
                ItemPerPage = 3,
                Page = 1,
                SearchWord = string.Empty
            };

            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId).ToList();
            Assert.IsTrue(result.Count == default(int));
        }

        [Test]
        public void GetItemsFromUserCartByPageNumberSuccessfully()
        {
            string orderId = "firstOrder";

            var filterObj = new FilterMenuItem
            {
                CategoryId = string.Empty,
                FilterOpt = string.Empty,
                ItemPerPage = 3,
                Page = 1,
                SearchWord = string.Empty
            };

            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId).ToArray();
            Assert.AreEqual(filterObj.ItemPerPage, result.Length);
        }

        [Test]
        public void GetItemsFromUserCartByPriceAscendingSuccessfully()
        {
            string orderId = "firstOrder";
            var filterObj = new FilterMenuItem
            {
                CategoryId = string.Empty,
                FilterOpt = "asc",
                ItemPerPage = 3,
                Page = 1,
                SearchWord = string.Empty
            };

            PurchaseService purchaseService = new PurchaseService(foodDeliveryUnitOfWork);
            var result = purchaseService.GetPurchaseItemsByFilters(filterObj, orderId);
            var resultIds = result.Select(r => r.Id).ToArray();
            var resultOrderedByDescIds = result.OrderBy(r => r.Price).Select(r => r.Id).ToArray();

            bool isValidResult = true;
            for (int i = 0; i < filterObj.ItemPerPage; i++)
            {
                isValidResult = resultIds[i] == resultOrderedByDescIds[i];
            }

            Assert.IsTrue(isValidResult);
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
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100, Category = new Category { Id = "firstCategoryId" } },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200, Category = new Category{ Id = "firstCategoryId" } },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300, Category = new Category{ Id = "firstCategoryId" } },
                new MenuItem { Id = "fourthMenuItemId", Name = "fourthMenuItemName", Description = "fourthMenuItemDescription", Price = 400, Category = new Category{ Id = "secondCategoryId" } },
                new MenuItem { Id = "fifthMenuItemId", Name = "fifthMenuItemName", Description = "fifthMenuItemDescription", Price = 500, Category = new Category{ Id = "secondCategoryId" } }
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
