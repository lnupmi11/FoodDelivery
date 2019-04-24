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
    public class PurchaceController : Controller
    {
        IPurchaceService _purchaceService;
        public PurchaceController(IPurchaceService purchaceService)
        {
            _purchaceService = purchaceService;
        }

        [Authorize]
        public IActionResult AllPurchaces()
        {
            var userName = User.Identity.Name;
            var purchaces = _purchaceService.GetListOfPurchaces(userName);
            return View(purchaces);
        }

        [Authorize]
        public IActionResult ItemsInSelectedPurchace(string purchaceId)
        {
            var purchaceItems = _purchaceService.GetPurchaceItems(purchaceId);
            return View(purchaceItems);
        }
    }
}