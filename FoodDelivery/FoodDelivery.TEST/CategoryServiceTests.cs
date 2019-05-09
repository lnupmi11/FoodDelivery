using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Menu;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.TEST
{
    class CategoryServiceTest
    {
        IUnitOfWork _unitOfWork;
        IList<Category> _categories;

        [SetUp]
        public void Setup()
        {
            _categories = new List<Category>
            {
                new Category{Id = "firstId", Description="Description1", CategoryName="Drinks"},
                new Category{Id = "secondId", Description="Description2", CategoryName="Pizza"},
                new Category{Id = "thirdId", Description="Description3", CategoryName="Sushi"},
            };

            var discountRepositoryMock = new Mock<IRepository<Category>>();
            discountRepositoryMock.Setup(repository => repository.GetQuery()).Returns(_categories.AsQueryable());
            discountRepositoryMock.Setup(repository => repository.Get(It.IsAny<string>())).Returns((string id) => _categories.FirstOrDefault(i => i.Id == id));
            discountRepositoryMock.Setup(repository => repository.Create(It.IsAny<Category>())).Callback((Category o) => _categories.Add(o));
            discountRepositoryMock.Setup(repository => repository.Update(It.IsAny<Category>())).Callback((Category o) => _categories[_categories.ToList().FindIndex(i => i.Id == o.Id)] = o);
            discountRepositoryMock.Setup(repository => repository.Delete(It.IsAny<string>())).Callback((string id) => _categories.Remove(_categories.FirstOrDefault(i => i.Id == id)));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(ufw => ufw.CategoriesRepository).Returns(discountRepositoryMock.Object);

            _unitOfWork = unitOfWorkMock.Object;
        }

        [Test]
        public void GetdiscountTest()
        {
            string getId = "firstId";
            var discount = _categories.AsQueryable();
            var expecteddiscount = _categories.FirstOrDefault(i => i.Id == getId);

            var CategoryService = new CategoryService(_unitOfWork);
            var actualItem = CategoryService.Get(getId);


            Assert.AreEqual(expecteddiscount.Id, actualItem.Id);
        }

        [Test]
        public void GetAllCategoriesTest()
        {
            var categories = _categories.AsQueryable();
            var expectedCount = categories.Count();

            var CategoryService = new CategoryService(_unitOfWork);
            var actualCount = CategoryService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void AdddiscountTest()
        {
            var newdiscount = new CategoryDTO()
            {
                Id="fourthId",
                Description="Description3",
                CategoryName="HotDrinks"
            };

            int startCount = _unitOfWork.CategoriesRepository.GetQuery().Count();

            CategoryService CategoryService = new CategoryService(_unitOfWork);
            CategoryService.Add(newdiscount);

            int finalCount = _unitOfWork.CategoriesRepository.GetQuery().Count();
            Assert.AreEqual(startCount + 1, finalCount);

            Category discount = _unitOfWork.CategoriesRepository.Get(newdiscount.Id);
            Assert.AreEqual(discount.Id, newdiscount.Id);
        }

        [Test]
        public void UpdatediscountTest()
        {
            var CategoryService = new CategoryService(_unitOfWork);
            string toUpdateId = "secondId";
            var toUpdate = CategoryService.Get(toUpdateId);
            toUpdate.CategoryName = "Pasta";
            CategoryService.Update(toUpdate);
            var actualItem = CategoryService.Get(toUpdateId);

            Assert.AreEqual(toUpdate.CategoryName, actualItem.CategoryName);
        }

        [Test]
        public void DeletediscountTest()
        {
            var categories = _categories.AsQueryable();
            var expectedCount = categories.Count() - 1;

            var CategoryService = new CategoryService(_unitOfWork);
            var toRemoveId = "secondId";
            var toRemove = CategoryService.Get(toRemoveId);
            CategoryService.Delete(toRemove);
            var actualCount = CategoryService.GetAll().Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
