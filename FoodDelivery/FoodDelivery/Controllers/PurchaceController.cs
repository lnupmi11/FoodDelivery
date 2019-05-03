using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class PurchaseController : Controller
    {
        IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [Authorize]
        public IActionResult AllPurchases()
        {
            var userName = User.Identity.Name;
            var purchases = _purchaseService.GetListOfPurchases(userName);
            return View(purchases);
        }

        [Authorize]
        public IActionResult ItemsInSelectedPurchase(string purchaseId)
        {
            var purchaseItems = _purchaseService.GetPurchaseItems(purchaseId);
            return View(purchaseItems);
        }
    }
}