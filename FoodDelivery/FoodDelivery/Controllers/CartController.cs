using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
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

        public IActionResult Index(int page = 1, string searchWord = "", string filterOpt = "", string categoryId = "")
        {
            const int itemsPerPage = 3;
            var userName = User.Identity.Name;
            CartModel cartModel = new CartModel();
            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            double totalPrice;

            if (!string.IsNullOrEmpty(userName))
            {
                cartItems = _basketService.GetUserBasketByFilters(page,searchWord,filterOpt,categoryId, userName, itemsPerPage).ToList();
                ViewBag.Total = Math.Ceiling(_basketService.GetAllUserBasketItems(userName).Count() * 1.0 /itemsPerPage);
                totalPrice = _basketService.GetTotalPriceOfUserBasketItems(userName);
                ViewBag.Page = page;
            }
            else
            {
                string allItems = HttpContext.Request.Cookies["menuItems"];
                Dictionary<string, string> itemsDictionary = string.IsNullOrEmpty(allItems) ? new Dictionary<string, string>() : ComplexCookiesExtension.FromComplexCookieString(allItems);
                cartItems = _basketService.GetUnAuthorizeUserBasketItems(itemsDictionary, page, searchWord, filterOpt, categoryId, itemsPerPage).ToList();
                ViewBag.Total = Math.Ceiling(itemsDictionary.Count() * 1.0 / itemsPerPage);
                totalPrice = _basketService.GetTotalPriceOfUnAuthorizeUserBasketItems(itemsDictionary);
                ViewBag.Page = page;
            }

            cartModel.TotalPrice = totalPrice;
            cartModel.Categories = _categoryService.GetAll();
            cartModel.CartItems = cartItems;

            ViewBag.FilterOpt = filterOpt;
            ViewBag.SearchWord = searchWord;
            ViewBag.CategoryId = categoryId;

            return View(cartModel);
        }

        public IActionResult AddItem(string itemId)
        {
            var userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                _basketService.AddItemToBasket(userName, itemId);
            }
            else
            {
                string allItems = HttpContext.Request.Cookies["menuItems"];
                Dictionary<string, string> itemsDictionary = string.IsNullOrEmpty(allItems) ? new Dictionary<string, string>() : ComplexCookiesExtension.FromComplexCookieString(allItems);
                if (itemsDictionary == null)
                {
                    itemsDictionary = new Dictionary<string, string>();
                    itemsDictionary.Add(itemId, "1");
                }
                else
                {
                    string itemCountString;
                    itemsDictionary.TryGetValue(itemId, out itemCountString);

                    if (string.IsNullOrEmpty(itemCountString))
                    {
                        itemsDictionary.Add(itemId, "1");
                    }
                    else
                    {
                        int itemCountInt = int.Parse(itemCountString);
                        itemCountInt++;
                        itemsDictionary[itemId] = itemCountInt.ToString();
                    }
                }
                HttpContext.Response.Cookies.Append("menuItems", ComplexCookiesExtension.ToComplexCookieString(itemsDictionary));
            }
            return new RedirectToRouteResult(new { controller = "Cart", action = "Index" });
        }

        public IActionResult RemoveItem(string itemId)
        {
            var userName = User.Identity.Name;
            if(!string.IsNullOrEmpty(userName))
            {
                _basketService.DeleteItemFromBasket(userName, itemId);
            }
            else
            {
                string allItems = HttpContext.Request.Cookies["menuItems"];
                Dictionary<string, string> itemsDictionary = string.IsNullOrEmpty(allItems) ? new Dictionary<string, string>() : ComplexCookiesExtension.FromComplexCookieString(allItems);
                if (itemsDictionary != null)
                {
                    string itemCountString;
                    itemsDictionary.TryGetValue(itemId, out itemCountString);

                    if (!string.IsNullOrEmpty(itemCountString))
                    {
                        int itemCountInt = int.Parse(itemCountString);
                        itemCountInt--;
                        if (itemCountInt == 0)
                        {
                            itemsDictionary.Remove(itemId);
                        }
                        else
                        {
                            itemsDictionary[itemId] = itemCountInt.ToString();
                        }
                        HttpContext.Response.Cookies.Append("menuItems", ComplexCookiesExtension.ToComplexCookieString(itemsDictionary));
                    }
                }
            }
            return new RedirectToRouteResult(new { controller = "Cart", action = "Index" });
        }

        [HttpGet]
        public IActionResult Submit()
        {
            var userName = User.Identity.Name;
            var submitCartDTO = new PreSubmitCartDTO();
            if(!string.IsNullOrEmpty(userName))
            {
                submitCartDTO.SavedAddresses = _userService.GetSavedAddresses(userName).ToList();
                submitCartDTO.IsAuthorize = true;
            }
            else
            {
                submitCartDTO.IsAuthorize = false;
            }
            submitCartDTO.Regions = _userService.GetRegions().ToList();
            return View(submitCartDTO);
        }

        [HttpPost]
        public IActionResult SubmitOnNewAddres(AddressDTO address, string additionalInfo, int paymentType)
        {
            var userName = User.Identity.Name;
            if(!string.IsNullOrEmpty(userName))
            {
                _userService.AddSavedAddress(userName, address);
                var addressId = _userService.GetSavedAddressId(address);
                _basketService.SubmitBasket(userName, addressId, paymentType);
                return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
            }
            else
            {
                _userService.AddAddressOfUnauthorizeUser(address);
                var addressId = _userService.GetSavedAddressId(address); 
                string allItems = HttpContext.Request.Cookies["menuItems"];
                Dictionary<string, string> itemsDictionary = string.IsNullOrEmpty(allItems) ? new Dictionary<string, string>() : ComplexCookiesExtension.FromComplexCookieString(allItems);
                _basketService.SubmitUnauthorizeUserBasket(itemsDictionary, addressId, paymentType);
                HttpContext.Response.Cookies.Append("menuItems", string.Empty);
                return View("~/Views/Cart/SuccessSubmit.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitOnSavedAddres(string addressId, string additionalInfo, int paymentType)
        {
            var userName = User.Identity.Name;
            _basketService.SubmitBasket(userName, addressId, paymentType);
            return new RedirectToRouteResult(new { controller = "Purchase", action = "AllPurchases" });
        }

        public IActionResult Clear()
        {
            var userName = User.Identity.Name;
            if (userName != null)
            {
                _basketService.ClearBasket(userName);
            }
            else
            {
                HttpContext.Response.Cookies.Append("menuItems", string.Empty);
            }
            return RedirectToRoute(new { controller = "Menu", action = "Index" });
        }
    }
}