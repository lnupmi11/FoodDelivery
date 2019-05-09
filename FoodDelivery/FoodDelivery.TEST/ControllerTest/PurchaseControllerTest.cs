using FoodDelivery.BLL.Interfaces;
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
        IPurchaseService purchaseService;
        [SetUp]
        public void Setup()
        {
            var purchaseServiceMock = new Mock<IPurchaseService>();
            purchaseServiceMock.Setup(service => service.GetListOfPurchases(It.IsAny<string>())).Returns(GetListOfPurchases());
            purchaseServiceMock.Setup(service => service.GetPurchaseItemsByFilters(It.IsAny<FilterMenuItem>(), It.IsAny<string>())).Returns(GetPurchaseItemsByFilters());
            purchaseService = purchaseServiceMock.Object;
        }

        [Test]
        public void ViewPurchaseHistorySuccssesfully()
        {
            
        }

        public List<PurchaseDTO> GetListOfPurchases()
        {
            return new List<PurchaseDTO>();
        }

        public List<PurchaseItemDTO> GetPurchaseItemsByFilters()
        {
            return new List<PurchaseItemDTO>();
        }
    }
}
