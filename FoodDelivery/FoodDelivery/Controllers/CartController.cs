using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.BLL.Interfaces;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        private List<CartItem> Carts;
        IOrderService orderService;

        public CartController(FoodDeliveryContext context) : base()
        {
            orderService = new OrderService(context);   
        }

        public IActionResult Index()
        {
            Carts = orderService.GetAllBasketItems("TestBasket").Select(i=>new CartItem { Id = i.Id, ItemTitle = i.Name, Discription = i.Description}).ToList();
            return View(Carts);
        }

        public IActionResult AddItem(string itemId)
        {
            orderService.AddItemToBasket("TestBasket", itemId);
            return new EmptyResult();
        }

        public IActionResult RemoveItem(string itemId)
        {
            // Use CartService to addcby id
            orderService.DeleteItemFromBasket("TestBasket", itemId);
            return new EmptyResult();
        }
    }
}