using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DTO.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class DiscountController : Controller
    {
        IDiscountService _discountService;

        public DiscountController(IDiscountService discountService) : base()
        {
            _discountService = discountService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_discountService.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(DiscountDTO modelToAdd)
        {
            _discountService.Add(modelToAdd);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var res = _discountService.Get(id);
            if (res != null)
            {
                return View(res);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(DiscountDTO newDataItem)
        {
            _discountService.Update(newDataItem);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var res = _discountService.Get(id);
            if (res != null)
            {
                return View(res);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(DiscountDTO newDataItem)
        {
            _discountService.Delete(newDataItem);
            return RedirectToAction("Index");
        }
    }
}
