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
                new MenuItem { Id = "firstMenuItemId", Name = "firstMenuItemName", Description = "firstMenuItemDescription", Price = 100, IsActive = true },
                new MenuItem { Id = "secondMenuItemId", Name = "secondMenuItemName", Description = "secondMenuItemDescription", Price = 200, IsActive = true },
                new MenuItem { Id = "thirdMenuItemId", Name = "thirdMenuItemName", Description = "thirdMenuItemDescription", Price = 300, IsActive = true },
                new MenuItem { Id = "fourthMenuItemId", Name = "fourthMenuItemName", Description = "fourthMenuItemDescription", Price = 400, IsActive = true },
                new MenuItem { Id = "fifthMenuItemId", Name = "fifthMenuItemName", Description = "fifthMenuItemDescription", Price = 500, IsActive = true }
            };

            var menuItemRepositoryMock = new Mock<IRepository<MenuItem>>();
            menuItemRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_menuItems.AsQueryable());
            menuItemRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string menuItemId) => _menuItems.FirstOrDefault(i => i.Id == menuItemId));
            menuItemRepositoryMock.Setup(repository => repository.Create(It.IsAny<MenuItem>())).Callback((MenuItem mi) => _menuItems.Add(mi));
            menuItemRepositoryMock.Setup(repository => repository.Update(It.IsAny<MenuItem>())).Callback((MenuItem mi) => _menuItems[_menuItems.ToList().FindIndex(i => i.Id == mi.Id)] = mi);
            menuItemRepositoryMock.Setup(repository => repository.Delete(It.IsAny<string>())).Callback((string id) => _menuItems.FirstOrDefault(i => i.Id == id).IsActive = false);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.MenuItemsRepository).Returns(menuItemRepositoryMock.Object);


            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void GetMenuItemTest()
        {
            string getId = "fourthMenuItemId";
            var menuItems = _menuItems.AsQueryable();
            var expectedItem = _menuItems.FirstOrDefault(i => i.Id == getId);

            var menuService = new MenuService(_unitOfWork);
            var actualItem = menuService.Get(getId);


            Assert.AreEqual(expectedItem.Id, actualItem.Id);
            Assert.AreEqual(expectedItem.Price, actualItem.Price);
            Assert.AreEqual(expectedItem.Name, actualItem.Name);
        }

        [Test]
        public void GetAllMenuItemsTest()
        {
            var menuItems = _menuItems.AsQueryable();
            var expectedCount = menuItems.Count();

            var menuService = new MenuService(_unitOfWork);
            var actualCount = menuService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
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
        public void UpdateMenuItemTest()
        {
            var menuService = new MenuService(_unitOfWork);
            string toUpdateId = "fifthMenuItemId";
            var toUpdate = menuService.Get(toUpdateId);
            toUpdate.Price = 333;
            menuService.Update(toUpdate);
            var actualItem = menuService.Get(toUpdateId);

            Assert.AreEqual(toUpdate.Price, actualItem.Price);
        }

        [Test]
        public void DeleteMenuItemTest()
        {
            var menuItems = _menuItems.AsQueryable();

            var menuService = new MenuService(_unitOfWork);
            var toRemoveId = "fourthMenuItemId";
            var toRemove = menuService.Get(toRemoveId);
            menuService.Delete(toRemove);
            var result = menuService.GetAll().ToArray();
            Assert.IsFalse(result.Any(i=>i.Id == toRemoveId));
        }

        [Test]
        public void GetMenuPageWithoutAnyFiltersTest1()
        {
            int page1 = 1;
            int pageSize = 2;
            int pageCount = 0;
            string sortOpt = "";
            string searchOpt = "";
            string categoryId = "";
            string discountId = "";

            var menuService = new MenuService(_unitOfWork);
            var actual = menuService.GetMenuPage(page1, pageSize, out pageCount, sortOpt, searchOpt, categoryId, discountId);
            var expected = _menuItems.Take(pageSize);
            var expectedPageCount = (int)Math.Ceiling((double)_menuItems.Count() / pageSize);

            Assert.AreEqual(expected.Count(), actual.Count());
            for(int i = 0;i <pageSize; ++i)
            {
                Assert.AreEqual(expected.ElementAt(i).Id, actual.ElementAt(i).Id);
            }
            Assert.AreEqual(expectedPageCount, pageCount);
        }

        [Test]
        public void GetMenuPageWithoutAnyFiltersTest2()
        {
            int page3 = 3;
            int pageSize = 2;
            int pageCount = 0;
            string sortOpt = "";
            string searchOpt = "";
            string categoryId = "";
            string discountId = "";

            var menuService = new MenuService(_unitOfWork);
            var actual = menuService.GetMenuPage(page3, pageSize, out pageCount, sortOpt, searchOpt, categoryId, discountId);
            var expected = _menuItems.Skip((page3 - 1) * pageSize).Take(pageSize);
            var expectedPageCount = (int)Math.Ceiling((double)_menuItems.Count() / pageSize);

            Assert.AreEqual(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); ++i)
            {
                Assert.AreEqual(expected.ElementAt(i).Id, actual.ElementAt(i).Id);
            }
            Assert.AreEqual(expectedPageCount, pageCount);
        }

        [Test]
        public void GetMenuPageAscFiltersTest()
        {
            int page1 = 1;
            int pageSize = 2;
            int pageCount = 0;
            string sortOpt = "asc";
            string searchOpt = "";
            string categoryId = "";
            string discountId = "";

            var menuService = new MenuService(_unitOfWork);
            var actual = menuService.GetMenuPage(page1, pageSize, out pageCount, sortOpt, searchOpt, categoryId, discountId);
            var expected = _menuItems.Take(pageSize).OrderBy(i => i.Price);
            var expectedPageCount = (int)Math.Ceiling((double)_menuItems.Count() / pageSize);

            Assert.AreEqual(expected.Count(), actual.Count());
            for (int i = 0; i < pageSize; ++i)
            {
                Assert.AreEqual(expected.ElementAt(i).Id, actual.ElementAt(i).Id);
            }
            Assert.AreEqual(expectedPageCount, pageCount);
        }


        [Test]
        public void GetMenuPage2DescFiltersTest()
        {
            int page = 2;
            int pageSize = 2;
            int pageCount = 0;
            string sortOpt = "desc";
            string searchOpt = "";
            string categoryId = "";
            string discountId = "";

            var menuService = new MenuService(_unitOfWork);
            var actual = menuService.GetMenuPage(page, pageSize, out pageCount, sortOpt, searchOpt, categoryId, discountId);
            var expected = _menuItems.OrderByDescending(i => i.Price).Skip((page - 1) * pageSize).Take(pageSize);
            var expectedPageCount = (int)Math.Ceiling((double)_menuItems.Count() / pageSize);

            Assert.AreEqual(expected.Count(), actual.Count());
            for (int i = 0; i < expected.Count(); ++i)
            {
                Assert.AreEqual(expected.ElementAt(i).Id, actual.ElementAt(i).Id);
            }
            Assert.AreEqual(expectedPageCount, pageCount);
        }

        [Test]
        public void GetMenuPageSearchFiltersTest()
        {
            int page1 = 1;
            int pageSize = 2;
            int pageCount = 0;
            string sortOpt = "";
            string searchOpt = "thmenu";
            string categoryId = "";
            string discountId = "";

            var menuService = new MenuService(_unitOfWork);
            var actual = menuService.GetMenuPage(page1, pageSize, out pageCount, sortOpt, searchOpt, categoryId, discountId);
            var expected = _menuItems.Where(i => i.Name.ToLower().Contains(searchOpt.ToLower().Trim())).Take(pageSize);
            var expectedPageCount = (int)Math.Ceiling((double)expected.Count() / pageSize);

            Assert.AreEqual(expected.Count(), actual.Count());
            for (int i = 0; i < pageSize; ++i)
            {
                Assert.AreEqual(expected.ElementAt(i).Id, actual.ElementAt(i).Id);
            }
            Assert.AreEqual(expectedPageCount, pageCount);
        }
    }
}
