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
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace FoodDelivery.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IMenuService _menuService;

        public MenuController(IMenuService menuService, IHostingEnvironment appEnvironment) : base()
        {
            _menuService = menuService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string sort = "none")
        {
            const int kItemsPerPage = 6;
            const int kRows = 2;
            ViewBag.Rows = kRows;
            ViewBag.Cols = kItemsPerPage / kRows;
            ViewBag.Page = page;
            ViewBag.Total = Math.Ceiling(_menuService.GetCount() / (double)kItemsPerPage);
            var result =  _menuService.GetPaginated(page, kItemsPerPage);
            switch(sort)
            {
                case "asc":
                    result = result.OrderBy(i => i.Price);
                    break;
                case "desc":
                    result = result.OrderByDescending(i => i.Price);
                    break;
            }
            return View(result);
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
            AddImage(modelToAdd);
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
            AddImage(newDataItem);
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

        private void AddImage(MenuItemDTO model)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files.ElementAt(0);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    var FileExtension = Path.GetExtension(fileName);
                    var physicalWebRootPath = _appEnvironment.ContentRootPath;
                    var newFileName = myUniqueFileName + FileExtension;
                    fileName = "wwwroot\\images\\menu" + $@"\{newFileName}";
                    model.Image = "images\\menu\\" + newFileName;

                    using (FileStream fs = System.IO.File.Create($"{physicalWebRootPath}\\{fileName}"))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
        }
    }
}