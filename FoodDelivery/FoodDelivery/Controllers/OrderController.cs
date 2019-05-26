using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DTO.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index(string status = "")
        {
            var orders = _orderService.GetByStatus(status);    

            return View(orders);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var order = _orderService.Get(id);

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(OrderDTO order)
        {
            _orderService.Update(order);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateOrderStatus(string id, string status)
        {
            _orderService.UpdateOrderStatus(id, status);
            return View("Index", _orderService.GetByStatus(""));
        }
    }
}