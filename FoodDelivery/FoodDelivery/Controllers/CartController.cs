using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        IBasketService _orderService;
        IUserService _userService;

        public CartController(IBasketService orderService, IUserService userService) : base()
        {
            _orderService = orderService;
            _userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            if (!string.IsNullOrEmpty(userName))
            {
                cartItems = _orderService.GetAllUserBasketItems(userName).ToList();
            }
            return View(cartItems);
        }

        [Authorize]
        public IActionResult AddItem(string itemId)
        {
            var userName = User.Identity.Name;
            _orderService.AddItemToBasket(userName, itemId);
            return new EmptyResult();
        }

        [Authorize]
        public IActionResult RemoveItem(string itemId)
        {
            var userName = User.Identity.Name;
            _orderService.DeleteItemFromBasket(userName, itemId);
            return new EmptyResult();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Submit()
        {
            var userName = User.Identity.Name;
            var submitCartDTO = new PreSubmitCartDTO();
            submitCartDTO.SavedAddresses = _userService.GetSavedAddresses(userName).ToList();
            submitCartDTO.Regions = _userService.GetRegions().ToList();
            return View(submitCartDTO);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitOnNewAddres(AddressDTO address, string additionalInfo, int paymentType)
        {
            var userName = User.Identity.Name;
            _userService.AddSavedAddress(userName, address);
            var addressId = _userService.GetSavedAddressId(address);
            _orderService.SubmitBasket(userName, addressId, paymentType);
            return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitOnSavedAddres(string addressId, string additionalInfo, int paymentType)
        {
            var userName = User.Identity.Name;
            _orderService.SubmitBasket(userName, addressId, paymentType);
            return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
        }

        [Authorize]
        public IActionResult Clear()
        {
            var userName = User.Identity.Name;
            _orderService.ClearBasket(userName);
            return RedirectToRoute(new { controller = "Menu", action = "Index" });
        }

    }
}