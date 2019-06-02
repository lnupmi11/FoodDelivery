using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Controllers;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Purchase;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;

namespace FoodDelivery.TEST.ControllerTest
{
    class PurchaseControllerTest
    {
        private IPurchaseService _purchaseService;
        private ICategoryService _categoryService;
        private ControllerContext _controllerContext;
        [SetUp]
        public void Setup()
        {
            var purchaseServiceMock = new Mock<IPurchaseService>();
            purchaseServiceMock.Setup(service => service.GetFilteredListOfPurchases(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(ListOfPurchases);
            purchaseServiceMock.Setup(service => service.GetPurchaseItems(It.IsAny<string>())).Returns(PurchaseItemsByFilters);
            purchaseServiceMock.Setup(service => service.GetPurchaseItemsByFilters(It.IsAny<FilterMenuItem>(), It.IsAny<string>())).Returns(PurchaseItemsByFilters);
            _purchaseService = purchaseServiceMock.Object;

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
        public void ViewPurchaseHistorySuccssesfully()
        {
            var contoller = new PurchaseController(_purchaseService, _categoryService);
            contoller.ControllerContext = _controllerContext;

            var result = contoller.AllPurchases() as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as List<PurchaseDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(model, ListOfPurchases);
        }

        [Test]
        public void ViewPurchaseDetailsSuccssesfully()
        {
            var contoller = new PurchaseController(_purchaseService, _categoryService);
            contoller.ControllerContext = _controllerContext;

            var result = contoller.ItemsInSelectedPurchase("purchaseId") as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as PurchaseModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.PurchaseItems.ToList(), PurchaseItemsByFilters);
            Assert.AreEqual(model.Categories, ListOfCategories);
        }

        public List<PurchaseDTO> ListOfPurchases = new List<PurchaseDTO>
         {
             new PurchaseDTO {Id="firstPurchase", PaymentType=0, SubmittedTime= DateTime.Now,TotalPrice=100 }
         };

        public List<CategoryDTO> ListOfCategories = new List<CategoryDTO>
        {
            new CategoryDTO{Id="firstCategoryId", CategoryName="FirstCategoryName", Description="descriptionFirst" },
            new CategoryDTO{Id="secondCategoryId", CategoryName="SecondCategoryName", Description="descriptionSecond" }
        };

        public List<PurchaseItemDTO> PurchaseItemsByFilters = new List<PurchaseItemDTO>
        {
            new PurchaseItemDTO{Count=1, Id="1",Description="descriptionFirst",Image="image1",Name="Name1",Price=150 },
            new PurchaseItemDTO{Count=2, Id="2",Description="descriptionSecond",Image="image2",Name="Name2",Price=250 },
        };
    }
}
