using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Controllers
{
    public class MenuController : Controller
    {
        IMenuService _menuService;

        public MenuController(IMenuService menuService) : base()
        {
            _menuService = menuService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_menuService.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Add(MenuItemDTO modelToAdd)
        {
            _menuService.Add(modelToAdd);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(string id)
        {
            var res = _menuService.Get(id);
            if (res != null)
            {
                return View(res);
            }
            ///TODO:
            ///Change to some Errro view
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(MenuItemDTO newDataItem)
        {
            _menuService.Update(newDataItem);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var res = _menuService.Get(id);
            if (res != null)
            {
                return View(res);
            }
            ///TODO:
            ///Change to some Errro view
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(MenuItemDTO newDataItem)
        {
            _menuService.Delete(newDataItem);
            return RedirectToAction("Index");
        }
    }
}