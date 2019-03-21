using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class MenuController : Controller
    {
        List<MenuItemModel> menuItems;

        public MenuController() : base()
        {
            menuItems = new List<MenuItemModel>
            {
                 new MenuItemModel { Name = "Item1", Price = 100, Description = "Description1" },
                 new MenuItemModel { Name = "Item2", Price = 150, Description = "Description2" },
                 new MenuItemModel { Name = "Item3", Price = 200, Description = "Description3" },
                 new MenuItemModel { Name = "Item4", Price = 250, Description = "Description4" },
                 new MenuItemModel { Name = "Item5", Price = 300, Description = "Description5" },
                 new MenuItemModel { Name = "Item6", Price = 350, Description = "Description6" },
                 new MenuItemModel { Name = "Item7", Price = 400, Description = "Description7" },
            };
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(menuItems);
        }

        [HttpGet]
        public IActionResult AddMenuItem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMenuItem(MenuItemModel modelToAdd)
        {
            menuItems.Add(modelToAdd);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditMenuItem(string name)
        {
            var menuItem = menuItems.FirstOrDefault(m => m.Name == name);
            if (menuItem != null)
            {
                return View(menuItem);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditMenuItem(MenuItemModel newDataItem)
        {
            var menuItem = menuItems.FirstOrDefault(m => m.Name == newDataItem.Name);
            if (menuItem != null)
            {
                menuItem.Description = newDataItem.Description;
                menuItem.Price = newDataItem.Price;
            }
            return RedirectToAction("Index");
        }
    }
}