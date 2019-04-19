using System.Collections.Generic;
using System.Linq;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        IBasketService _orderService;

        public CartController(IBasketService orderService) : base()
        {
            _orderService = orderService;   
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
    }
}