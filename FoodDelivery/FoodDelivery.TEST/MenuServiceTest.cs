using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO.Menu;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class MenuServiceTest
    {
        IUnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            var menuItemRepositoryMock = new Mock<IRepository<MenuItem>>();
            menuItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(GetMenuItemRepositoryQuery());
            menuItemRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string menuItemId) => GetMenuItem(menuItemId));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.MenuItemsRepository).Returns(menuItemRepositoryMock.Object);

            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void AddMenuItemsSuccessfully()
        {
            var newItem = new MenuItemDTO()
            {
                Id = "New id",
                Price = 100,
                Description = "Description",
                Name = "Item1"
            };

            int startCount = _unitOfWork.MenuItemsRepository.GetQuery().Count();

            MenuService menuService = new MenuService(_unitOfWork);
            menuService.Add(newItem);

            int finalCount = _unitOfWork.MenuItemsRepository.GetQuery().Count();
            Assert.AreEqual(startCount + 1, finalCount);
        }

        private IQueryable<MenuItem> GetMenuItemRepositoryQuery()
        {
            return GetMenuItems().AsQueryable();
        }

        private List<MenuItem> GetMenuItems()
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

        private MenuItem GetMenuItem(string itemId)
        {
            return GetMenuItems().FirstOrDefault(i => i.Id == itemId);
        }
    }
}
