using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Controllers;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Purchase;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.TEST.ControllerTest
{
    class PurchaseControllerTest
    {
        private IPurchaseService _purchaseService;
        private ICategoryService _categoryService;
        [SetUp]
        public void Setup()
        {
            var purchaseServiceMock = new Mock<IPurchaseService>();
            purchaseServiceMock.Setup(service => service.GetListOfPurchases(It.IsAny<string>())).Returns(GetListOfPurchases());
            purchaseServiceMock.Setup(service => service.GetPurchaseItemsByFilters(It.IsAny<FilterMenuItem>(), It.IsAny<string>())).Returns(GetPurchaseItemsByFilters());
            _purchaseService = purchaseServiceMock.Object;
        }

        [Test]
        public void ViewPurchaseHistorySuccssesfully()
        {
            var contoller = new PurchaseController(_purchaseService, _categoryService);
            //var result = contoller.AllPurchases();
        }

        public List<PurchaseDTO> GetListOfPurchases()
        {
            return new List<PurchaseDTO>
            {
                new PurchaseDTO {Id="firstPurchase", PaymentType=0, SubmittedTime= DateTime.Now,TotalPrice=100 }
            };
        }

        public List<PurchaseItemDTO> GetPurchaseItemsByFilters()
        {
            return new List<PurchaseItemDTO>();
        }
    }
}
