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
        IList<MenuItem> _menuItems;

        [SetUp]
        public void Setup()
        {
            _menuItems = new List<MenuItem>
            {
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100 },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200 },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300 },
                new MenuItem { Id = "fourthMenuItemId", Name = "fourthMenuItemName", Description = "fourthMenuItemDescription", Price = 400 },
                new MenuItem { Id = "fifthMenuItemId", Name = "fifthMenuItemName", Description = "fifthMenuItemDescription", Price = 500 }
            };

            var menuItemRepositoryMock = new Mock<IRepository<MenuItem>>();
            menuItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_menuItems.AsQueryable());
            menuItemRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string menuItemId) => _menuItems.FirstOrDefault(i => i.Id == menuItemId));
            menuItemRepositoryMock.Setup(repository => repository.Create(It.IsAny<MenuItem>())).Callback((MenuItem mi) => _menuItems.Add(mi));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.MenuItemsRepository).Returns(menuItemRepositoryMock.Object);


            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void AddMenuItemTest()
        {
            var newItem = new MenuItemDTO()
            {
                Id = "sixthMenuItem",
                Price = 5600,
                Description = "sixthMenuItemDescription",
                Name = "sixthMenuItemName"
            };

            int startCount = _unitOfWork.MenuItemsRepository.GetQuery().Count();

            MenuService menuService = new MenuService(_unitOfWork);
            menuService.Add(newItem);

            int finalCount = _unitOfWork.MenuItemsRepository.GetQuery().Count();
            Assert.AreEqual(startCount + 1, finalCount);

            MenuItem menuItem = _unitOfWork.MenuItemsRepository.Get(newItem.Id);
            Assert.AreEqual(menuItem.Id, newItem.Id);
        }

        [Test]
        public void GetPaginatedTest()
        {

        }
    }
}
