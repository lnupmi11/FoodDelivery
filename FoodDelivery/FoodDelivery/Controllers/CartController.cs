using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CartController : Controller
    {
        private List<CartItem> Carts;

        public CartController() : base()
        {
            Carts = new List<CartItem>
            {
                new CartItem { Count = 2, Id = 1, ItemTitle = "Item1", Discription="Description1"},
                new CartItem { Count = 1, Id = 2, ItemTitle = "Item2", Discription="Description2"},
                new CartItem { Count = 2, Id = 3, ItemTitle = "Item3", Discription="Description3"},
                new CartItem { Count = 3, Id = 4, ItemTitle = "Item4", Discription="Description4"},
                new CartItem { Count = 4, Id = 5, ItemTitle = "Item5", Discription="Description5"},
                new CartItem { Count = 5, Id = 6, ItemTitle = "Item6", Discription="Description6"}
            };
        }

        public IActionResult Index()
        {
            return View(Carts);
        }

        public IActionResult AddItem(int itemId)
        {
            // Use CartService to addcby id
            Carts.Add(new CartItem { Count = 1, Id = itemId, ItemTitle = "ItemFromMenuList" });
            return new EmptyResult();
        }

        public IActionResult RemoveItem(int itemId)
        {
            // Use CartService to addcby id
            var cart = Carts.FirstOrDefault(c=>c.Id == itemId);
            if(cart != null)
            {
                Carts.Remove(cart);
            }
            return new EmptyResult();
        }
    }
}