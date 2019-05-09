﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DTO.Purchase;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class PurchaseController : Controller
    {
        IPurchaseService _purchaseService;
        ICategoryService _categoryService;
        public PurchaseController(IPurchaseService purchaseService, ICategoryService categoryService)
        {
            _purchaseService = purchaseService;
            _categoryService = categoryService;
        }

        [Authorize]
        public IActionResult AllPurchases()
        {
            var userName = User.Identity.Name;
            var purchases = _purchaseService.GetListOfPurchases(userName);
            return View(purchases);
        }

        [Authorize]
        public IActionResult ItemsInSelectedPurchase(string purchaseId, int page = 1, string searchWord = "", string filterOpt = "", string categoryId = "")
        {
            var purchaseItems = _purchaseService.GetPurchaseItems(purchaseId);

            const int itemsPerPage = 3;
            var userName = User.Identity.Name;
            PurchaseModel purchaseModel = new PurchaseModel();
            List<PurchaseItemDTO> purchasedItems = new List<PurchaseItemDTO>();
            if (!string.IsNullOrEmpty(userName))
            {
                purchasedItems = _purchaseService.GetPurchaseItemsByFilters(page,searchWord,filterOpt,categoryId,itemsPerPage,purchaseId).ToList();
                ViewBag.Total = Math.Ceiling(_purchaseService.GetPurchaseItems(purchaseId).Count() * 1.0 / itemsPerPage);
                ViewBag.Page = page;
            }
            purchaseModel.PurchaseItems = purchasedItems;
            purchaseModel.Categories = _categoryService.GetAll();

            ViewBag.FilterOpt = filterOpt;
            ViewBag.SearchWord = searchWord;
            ViewBag.CategoryId = categoryId;
            ViewBag.PurchaseId = purchaseId;
            return View(purchaseModel);
        }
    }
}