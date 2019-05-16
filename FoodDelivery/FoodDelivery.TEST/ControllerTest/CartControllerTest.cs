using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Controllers;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace FoodDelivery.TEST.ControllerTest
{
    class CartControllerTest
    {
        private IBasketService _basketService;
        private IUserService _userService;
        private ICategoryService _categoryService;
        private ControllerContext _controllerContext;
        private Mock<IBasketService> _basketServiceMock; 

        [SetUp]
        public void Setup()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _basketServiceMock.Setup(service => service.AddItemToBasket(It.IsAny<string>(), It.IsAny<string>()));
            _basketServiceMock.Setup(service => service.DeleteItemFromBasket(It.IsAny<string>(), It.IsAny<string>()));
            _basketServiceMock.Setup(service => service.ClearBasket(It.IsAny<string>()));
            _basketService = _basketServiceMock.Object;

            var identityMock = new Mock<IIdentity>();
            identityMock.SetupGet(p => p.Name).Returns("firstUser");

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User.Identity).Returns(identityMock.Object);

            _controllerContext = new ControllerContext();
            _controllerContext.HttpContext = httpContextMock.Object;

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.GetAll()).Returns(() => ListOfCategories);
            _categoryService = categoryServiceMock.Object;
        }

        [Test]
        public void AddItemToCartSuccssesfully()
        {
            var contoller = new CartController(_basketService,_userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.AddItem("itemId") as EmptyResult;
            Assert.IsNotNull(result);

            _basketServiceMock.Verify(mock => mock.AddItemToBasket(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void DeleteItemFromCartSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.RemoveItem("itemId") as EmptyResult;
            Assert.IsNotNull(result);

            _basketServiceMock.Verify(mock => mock.DeleteItemFromBasket(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void ClearCartSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.Clear() as RedirectToRouteResult;
            Assert.IsNotNull(result);

            _basketServiceMock.Verify(mock => mock.ClearBasket(It.IsAny<string>()), Times.Once());
        }

        public List<CategoryDTO> ListOfCategories = new List<CategoryDTO>
        {
            new CategoryDTO{Id="firstCategoryId", CategoryName="FirstCategoryName", Description="descriptionFirst" },
            new CategoryDTO{Id="secondCategoryId", CategoryName="SecondCategoryName", Description="descriptionSecond" }
        };
    }
}
