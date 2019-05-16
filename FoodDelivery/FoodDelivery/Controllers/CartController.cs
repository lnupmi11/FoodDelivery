using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        IBasketService _basketService;
        IUserService _userService;
        ICategoryService _categoryService;

        public CartController(IBasketService orderService, IUserService userService, ICategoryService categoryService) : base()
        {
            _basketService = orderService;
            _userService = userService;
            _categoryService = categoryService;
        }

        [Authorize]
        public IActionResult Index(int page = 1, string searchWord = "", string filterOpt = "", string categoryId = "")
        {
            const int itemsPerPage = 3;
            var userName = User.Identity.Name;
            CartModel cartModel = new CartModel();
            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            if (!string.IsNullOrEmpty(userName))
            {
                cartItems = _basketService.GetUserBasketByFilters(page,searchWord,filterOpt,categoryId, userName, itemsPerPage).ToList();
                ViewBag.Total = Math.Ceiling(_basketService.GetAllUserBasketItems(userName).Count() * 1.0 /itemsPerPage);
                ViewBag.Page = page;
            }
            cartModel.CartItems = cartItems;
            cartModel.Categories = _categoryService.GetAll();

            ViewBag.FilterOpt = filterOpt;
            ViewBag.SearchWord = searchWord;
            ViewBag.CategoryId = categoryId;

            return View(cartModel);
        }

        [Authorize]
        public IActionResult AddItem(string itemId)
        {
            var userName = User.Identity.Name;
            _basketService.AddItemToBasket(userName, itemId);
            return new EmptyResult();
        }

        [Authorize]
        public IActionResult RemoveItem(string itemId)
        {
            var userName = User.Identity.Name;
            _basketService.DeleteItemFromBasket(userName, itemId);
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
            _basketService.SubmitBasket(userName, addressId, paymentType);
            return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitOnSavedAddres(string addressId, string additionalInfo, int paymentType)
        {
            var userName = User.Identity.Name;
            _basketService.SubmitBasket(userName, addressId, paymentType);
            return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
        }

        [Authorize]
        public IActionResult Clear()
        {
            var userName = User.Identity.Name;
            _basketService.ClearBasket(userName);
            return RedirectToRoute(new { controller = "Menu", action = "Index" });
        }
    }
}