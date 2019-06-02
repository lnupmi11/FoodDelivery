using NUnit.Framework;
using FoodDelivery.DAL.Interfaces;
using Moq;
using FoodDelivery.DAL.Models;
using FoodDelivery.BLL.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using FoodDelivery.DAL.Models.Enums;

namespace FoodDelivery.TEST
{
    class BasketServiceTest
    {
        IUnitOfWork foodDeliveryUnitOfWork;
        [SetUp]
        public void Setup()
        {
            Orders = new List<Order>();
            var userRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            userRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetUserRepositoryQuery());

            var menuItemRepositoryMock = new Mock<IRepository<MenuItem>>();
            menuItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetMenuItemRepositoryQuery());
            menuItemRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string menuItemId) => GetMenuItem(menuItemId));

            var orderRepositoryMock = new Mock<IRepository<Order>>();
            orderRepositoryMock.Setup(repository => repository.Create(It.IsAny<Order>())).Callback((Order order) => Orders.Add(order));

            var addressRepositoryMock = new Mock<IRepository<Address>>();
            addressRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetAddress()); ;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.UsersRepository).Returns(userRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.MenuItemsRepository).Returns(menuItemRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.OrdersRepository).Returns(orderRepositoryMock.Object);
            unitOfWorkMock.Setup(ufw => ufw.AddressesRepository).Returns(addressRepositoryMock.Object);
            foodDeliveryUnitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void AddSeveralTimeTheSameMenuItemInUserBasketSuccssesfully()
        {
            string userName = "firstTestUser";
            string menuItemId = "firstMenuItemId";

            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            int startCount = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId).Count;

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            basketService.AddItemToBasket(userName, menuItemId);

            int finalCount = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId).Count;
            Assert.AreEqual(startCount + 1, finalCount);
        }

        [Test]
        public void AddNewMenuItemInUserBasketSuccssesfully()
        {
            string userName = "secondTestUser";
            string menuItemId = "firstMenuItemId";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket)
                .Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            var menuItemInBasket = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId);
            Assert.AreEqual(null, menuItemInBasket);

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            basketService.AddItemToBasket(userName, menuItemId);

            menuItemInBasket = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId);

            var epectedResult = foodDeliveryUnitOfWork.MenuItemsRepository.Get(menuItemId);

            Assert.AreEqual(epectedResult.Id, menuItemInBasket.MenuItem.Id);
            Assert.AreEqual(menuItemInBasket.Count, 1);
        }

        [Test]
        public void AddNewMenuItemInUserBasketInvalidUserName()
        {
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            string userName = "thirdTestUser";
            string menuItemId = "firstMenuItemId";

            var ex = Assert.Throws<ArgumentException>(() => basketService.AddItemToBasket(userName, menuItemId));
            Assert.That(ex.Message, Is.EqualTo($"There is no user item with the following userName: {userName}"));
        }

        [Test]
        public void AddNewMenuItemInUserBasketInvalidMenuItemId()
        {
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            string menuItemId = "tenthMenuItemId";
            string userName = "firstUser";

            var ex = Assert.Throws<ArgumentException>(() => basketService.AddItemToBasket(userName, menuItemId));
            Assert.That(ex.Message, Is.EqualTo($"There is no menu item with the following id: {menuItemId}"));
        }

        [Test]
        public void DeleteMenuItemFromUserBasketSuccssesfully()
        {
            string userName = "firstTestUser";
            string menuItemId = "firstMenuItemId";

            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            int startCount = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId).Count;

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            basketService.DeleteItemFromBasket(userName, menuItemId);

            Assert.IsNull(user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId));
        }

        [Test]
        public void DeleteMenuItemFromUserBasketWhatDoesNotExistInBasket()
        {
            string userName = "secondTestUser";
            string menuItemId = "firstMenuItemId";

            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket)
                .Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            var menuItemInBasket = user.Basket.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId);
            Assert.AreEqual(null, menuItemInBasket);
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var ex = Assert.Throws<ArgumentException>(() => basketService.DeleteItemFromBasket(userName, menuItemId));
            Assert.That(ex.Message, Is.EqualTo($"There is no menu item in the user basket with the following id: {menuItemId}"));
        }

        [Test]
        public void DeleteMenuItemFromUserBasketInvalidUserName()
        {
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            string userName = "thirdTestUser";
            string menuItemId = "firstMenuItemId";

            var ex = Assert.Throws<ArgumentException>(() => basketService.DeleteItemFromBasket(userName, menuItemId));
            Assert.That(ex.Message, Is.EqualTo($"There is no user item with the following userName: {userName}"));
        }

        [Test]
        public void DeleteMenuItemFromUserBasketInvalidMenuItemId()
        {
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            string menuItemId = "tenthMenuItemId";
            string userName = "firstUser";

            var ex = Assert.Throws<ArgumentException>(() => basketService.DeleteItemFromBasket(userName, menuItemId));
            Assert.That(ex.Message, Is.EqualTo($"There is no menu item with the following id: {menuItemId}"));
        }

        [Test]
        public void SubmitCartSuccessfully()
        {
            string userName = "firstTestUser";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            var itemsIds = user.Basket.MenuItems.Select(mi => mi.MenuItemId);
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            basketService.SubmitBasket(userName, "testAddress", (int)PaymentType.Cash);
            Assert.IsTrue(!user.Basket.MenuItems.Any());
        }

        [Test]
        public void SubmitCartForNotExistingUser()
        {
            string userName = "firstUser";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var ex = Assert.Throws<ArgumentException>(() => basketService.SubmitBasket(userName, "testAddress", (int)PaymentType.Cash));
            Assert.That(ex.Message, Is.EqualTo($"There is no basket for user: { userName}"));
        }

        [Test]
        public void ClearCartSuccessfully()
        {
            string userName = "firstTestUser";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            basketService.ClearBasket(userName);
            Assert.IsTrue(!user.Basket.MenuItems.Any());
        }

        [Test]
        public void ClearCartForNotExistingUser()
        {
            string userName = "firstUser";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket).
                Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var ex = Assert.Throws<ArgumentException>(() => basketService.ClearBasket(userName));
            Assert.That(ex.Message, Is.EqualTo($"There is no basket with the following user: {userName}"));
        }

        [Test]
        public void GetAllItemsFromUserCartSuccessfully()
        {
            string userName = "firstTestUser";
            var user = foodDeliveryUnitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Basket)
                .Include(u => u.Basket.MenuItems).FirstOrDefault(u => u.UserName == userName);

            var userItemIds = user.Basket.MenuItems.Select(mi => mi.MenuItemId).ToList();
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetAllUserBasketItems(userName).Select(mi=>mi.Id);

            Assert.AreEqual(userItemIds.Count, userItemIds.Intersect(result).ToList().Count);
        }

        [Test]
        public void GetAllItemsFromNotExistingUserCart()
        {
            string userName = "firstUser";

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);

            var ex = Assert.Throws<ArgumentException>(() => basketService.GetAllUserBasketItems(userName));
            Assert.That(ex.Message, Is.EqualTo($"There is no user item with the following userName: {userName}"));
        }

        [Test]
        public void GetItemsFromUserCartByPriceDescendingSuccessfully()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, string.Empty, "desc", string.Empty, userName, elementCount);
            var resultIds = result.Select(r => r.Id).ToArray();
            var resultOrderedByDescIds = result.OrderByDescending(r => r.Price).Select(r => r.Id).ToArray();

            bool isValidResult = true;
            for (int i = 0; i < elementCount; i++)
            {
                isValidResult = resultIds[i] == resultOrderedByDescIds[i];
            }

            Assert.IsTrue(isValidResult);
        }

        [Test]
        public void GetItemsFromUserCartByItemNameSuccessfully()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;
            const string searchedItemName = "firstMenuItemId";

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, searchedItemName, "desc", string.Empty, userName, elementCount);
            Assert.IsTrue(result.All(r=>r.Name.Contains(searchedItemName)));
        }

        [Test]
        public void GetItemsFromUserCartByCategorySuccessfully()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;
            const string searchedCtegoryId = "firstCategoryId";
            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, string.Empty, "desc", searchedCtegoryId, userName, elementCount);
            Assert.IsTrue(result.All(r => r.Category.Id == searchedCtegoryId));
        }

        [Test]
        public void GetItemsFromUserCartByNotExistingCategory()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;
            const string searchedCtegoryId = "randomCategoryId";

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, string.Empty, "desc", searchedCtegoryId, userName, elementCount).ToList();
            Assert.IsTrue(result.Count == default(int));
        }

        [Test]
        public void GetItemsFromUserCartByPageNumberSuccessfully()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, string.Empty, "desc", string.Empty, userName, elementCount).ToArray();
            Assert.AreEqual(elementCount, result.Length);
        }

        [Test]
        public void GetItemsFromUserCartByPriceAscendingSuccessfully()
        {
            string userName = "firstTestUser";
            const int elementCount = 3;

            BasketService basketService = new BasketService(foodDeliveryUnitOfWork);
            var result = basketService.GetUserBasketByFilters(1, string.Empty, "asc", string.Empty, userName, elementCount);
            var resultIds = result.Select(r => r.Id).ToArray();
            var resultOrderedByDescIds = result.OrderBy(r => r.Price).Select(r => r.Id).ToArray();

            bool isValidResult = true;
            for (int i = 0; i < elementCount; i++)
            {
                isValidResult = resultIds[i] == resultOrderedByDescIds[i];
            }

            Assert.IsTrue(isValidResult);
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
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100, Category = new Category { Id="firstCategoryId"} },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200, Category = new Category { Id="firstCategoryId"} },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300, Category = new Category{ Id="secondCategoryId"} },
                new MenuItem { Id = "fourthMenuItemId", Name = "fourthMenuItemName", Description = "fourthMenuItemDescription", Price = 400, Category = new Category{ Id="secondCategoryId"} },
                new MenuItem { Id = "fifthMenuItemId", Name = "fifthMenuItemName", Description = "fifthMenuItemDescription", Price = 500, Category = new Category{ Id="secondCategoryId"} }
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

        public IQueryable<Address> GetAddress()
        {
            return new List<Address>
            {
                new Address{ Id = "testAddress"},
                new Address{ Id = "secondAddress"}
            }.AsQueryable();
        }

        public List<Order> Orders { get; set; }
    }
}
