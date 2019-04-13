using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class MenuController : Controller
    {
        List<MenuItemDTO> menuItems;

        public MenuController() : base()
        {
            menuItems = new List<MenuItemDTO>
            {
                 new MenuItemDTO { Name = "Item1", Price = 100, Description = "Description1", Id = "1" },
                 new MenuItemDTO { Name = "Item2", Price = 150, Description = "Description2", Id = "2" },
                 new MenuItemDTO { Name = "Item4", Price = 250, Description = "Description4", Id = "4" },
                 new MenuItemDTO { Name = "Item5", Price = 300, Description = "Description5", Id = "5" },
                 new MenuItemDTO { Name = "Item6", Price = 350, Description = "Description6", Id = "6" },
                 new MenuItemDTO { Name = "Item7", Price = 400, Description = "Description7", Id = "7" },
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
        public IActionResult AddMenuItem(MenuItemDTO modelToAdd)
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
        public IActionResult EditMenuItem(MenuItemDTO newDataItem)
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