using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Controllers;
using FoodDelivery.DTO.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace FoodDelivery.TEST.ControllerTest
{
    class CategoryControllerTest
    {
        private ICategoryService _categoryService;
        private ControllerContext _controllerContext;
        private Mock<ICategoryService> _categoryServiceMock;

        [SetUp]
        public void Setup()
        {
            var identityMock = new Mock<IIdentity>();
            identityMock.SetupGet(p => p.Name).Returns("firstUser");

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User.Identity).Returns(identityMock.Object);

            _controllerContext = new ControllerContext();
            _controllerContext.HttpContext = httpContextMock.Object;

            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryServiceMock.Setup(service => service.GetAll()).Returns(() => ListOfCategories);
            _categoryServiceMock.Setup(service => service.Get(It.IsAny<string>())).Returns(() => ListOfCategories.FirstOrDefault());
            _categoryServiceMock.Setup(service => service.Add(It.IsAny<CategoryDTO>()));
            _categoryService = _categoryServiceMock.Object;
        }

        [Test]
        public void GetAllCategoriesSuccssesfully()
        {
            var contoller = new CategoryController(_categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.Index() as ViewResult;
            Assert.IsNotNull(result);
            _categoryServiceMock.Verify(mock => mock.GetAll(), Times.Once);
            var categories = result.Model as List<CategoryDTO>;
            Assert.IsTrue(categories.TrueForAll(c => ListOfCategories.Any(lc => lc.Id == c.Id)));
        }

        [Test]
        public void AddCategorySuccssesfully()
        {
            var contoller = new CategoryController(_categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.Add(new CategoryDTO()) as RedirectToActionResult;
            Assert.IsNotNull(result);
            _categoryServiceMock.Verify(mock => mock.Add(It.IsAny<CategoryDTO>()), Times.Once);
        }

        [Test]
        public void EditCategorySuccsessfully()
        {
            var contoller = new CategoryController(_categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.Edit(string.Empty) as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as CategoryDTO;
            _categoryServiceMock.Verify(mock => mock.Get(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(model.Id, ListOfCategories.FirstOrDefault().Id);
        }

        public List<CategoryDTO> ListOfCategories = new List<CategoryDTO>
        {
            new CategoryDTO{Id="firstCategoryId", CategoryName="FirstCategoryName", Description="descriptionFirst" },
            new CategoryDTO{Id="secondCategoryId", CategoryName="SecondCategoryName", Description="descriptionSecond" }
        };

    }
}
