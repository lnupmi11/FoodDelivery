using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Controllers;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Purchase;
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
    class CartControllerTest
    {
        private IBasketService _basketService;
        private IUserService _userService;
        private ICategoryService _categoryService;
        private ControllerContext _controllerContext;
        private Mock<IBasketService> _basketServiceMock;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void Setup()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _basketServiceMock.Setup(service => service.AddItemToBasket(It.IsAny<string>(), It.IsAny<string>()));
            _basketServiceMock.Setup(service => service.DeleteItemFromBasket(It.IsAny<string>(), It.IsAny<string>()));
            _basketServiceMock.Setup(service => service.ClearBasket(It.IsAny<string>()));
            _basketServiceMock.Setup(service => service.SubmitBasket(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            _basketService = _basketServiceMock.Object;


            _userServiceMock = new Mock<IUserService>();
            _userServiceMock.Setup(service => service.GetRegions()).Returns(ListOfRegions);
            _userServiceMock.Setup(service => service.GetSavedAddresses(It.IsAny<string>())).Returns(ListOfAddresses);
            _userServiceMock.Setup(service => service.AddSavedAddress(It.IsAny<string>(), It.IsAny<AddressDTO>()));
            _userService = _userServiceMock.Object;

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
            var result = contoller.AddItem("itemId") as RedirectToRouteResult;
            Assert.IsNotNull(result);

            _basketServiceMock.Verify(mock => mock.AddItemToBasket(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void DeleteItemFromCartSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.RemoveItem("itemId") as RedirectToRouteResult;
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

        [Test]
        public void SubmitCartSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.Submit() as ViewResult;
            Assert.IsNotNull(result);
            var model = result.Model as PreSubmitCartDTO;
            Assert.IsTrue(model.Regions.All(r => ListOfRegions.Contains(r)));
            Assert.IsTrue(model.SavedAddresses.All(a => ListOfAddresses.Any(sa => sa.AddressId == a.AddressId)));
        }

        [Test]
        public void SubmitCartOnExistingAddressSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.SubmitOnSavedAddres(string.Empty, string.Empty, 0) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            _basketServiceMock.Verify(mock => mock.SubmitBasket(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void SubmitCartOnNewAddressSuccssesfully()
        {
            var contoller = new CartController(_basketService, _userService, _categoryService);
            contoller.ControllerContext = _controllerContext;
            var result = contoller.SubmitOnNewAddres(new AddressDTO(), string.Empty, 0) as RedirectToRouteResult;
            Assert.IsNotNull(result);
            _userServiceMock.Verify(mock => mock.AddSavedAddress(It.IsAny<string>(), It.IsAny<AddressDTO>()), Times.Once);
            _basketServiceMock.Verify(mock => mock.SubmitBasket(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        public List<CategoryDTO> ListOfCategories = new List<CategoryDTO>
        {
            new CategoryDTO{Id="firstCategoryId", CategoryName="FirstCategoryName", Description="descriptionFirst" },
            new CategoryDTO{Id="secondCategoryId", CategoryName="SecondCategoryName", Description="descriptionSecond" }
        };

        public List<string> ListOfRegions = new List<string>()
        {
            "firstRegion", "secondRegion", "thirdRegion"
        };

        public List<AddressDTO> ListOfAddresses = new List<AddressDTO>()
        {
            new AddressDTO { AddressId = "1", BuildingNumber=125,Region="firstRegion",Street="firstStreet"},
            new AddressDTO { AddressId = "2", BuildingNumber=127,Region="secondRegion",Street="secondStreet"},
            new AddressDTO { AddressId = "3", BuildingNumber=175,Region="firstRegion",Street="thirdStreet"},
            new AddressDTO { AddressId = "4", BuildingNumber=105,Region="thirdRegion",Street="fourthStreet"}
        };

    }
}
