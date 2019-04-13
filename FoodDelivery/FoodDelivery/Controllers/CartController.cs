using System.Linq;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        IOrderService _orderService;

        public CartController(IOrderService orderService) : base()
        {
            _orderService = orderService;   
        }

        [Authorize]
        public IActionResult Index()
        {
            var aaa = User.Identity.Name;
            var itemsInCart = _orderService.GetAllBasketItems("TestBasket").Select(i=>new CartItem { Id = i.Id, ItemTitle = i.Name, Discription = i.Description}).ToList();
            return View(itemsInCart);
        }

        public IActionResult AddItem(string itemId)
        {
            _orderService.AddItemToBasket("TestBasket", itemId);
            return new EmptyResult();
        }

        public IActionResult RemoveItem(string itemId)
        {
            // Use CartService to addcby id
            _orderService.DeleteItemFromBasket("TestBasket", itemId);
            return new EmptyResult();
        }
    }
}