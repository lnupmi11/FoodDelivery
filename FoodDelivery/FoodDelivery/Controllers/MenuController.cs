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
using FoodDelivery.Models;

namespace FoodDelivery.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IMenuService _menuService;
        ICategoryService _categoryService;

        public MenuController(IMenuService menuService, ICategoryService categoryService, IHostingEnvironment appEnvironment) : base()
        {
            _menuService = menuService;
            _categoryService = categoryService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string searchWord = "", string filterOpt="", string categoryId = "")
        {
            const int kPageSize = 6;
            const int kRows = 2;
            const int kCols = 3;
            ViewBag.Rows = kRows;
            ViewBag.Cols = kCols;
            ViewBag.Page = page;
            ViewBag.Total = Math.Ceiling(_menuService.GetPageCount(searchWord)/(double)kPageSize);

            ViewBag.FilterOpt = filterOpt;
            ViewBag.SearchWord = searchWord;
            ViewBag.CategoryId = categoryId;
            return View(new MenuModel
            {
                MenuItems = _menuService.GetMenuPage(page, kPageSize, filterOpt, searchWord, categoryId),
                Categories = _categoryService.GetAll()
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var res  = new AddMenuModel
            {
                MenuItemDTO = new MenuItemDTO
                {
                    Categories = new List<CategoryDTO>()
                },
                Categories = _categoryService.GetAll()
            };
            res.MenuItemDTO.Categories.Add(new CategoryDTO());
            return View(res);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Add(AddMenuModel modelToAdd)
        {
            AddImage(modelToAdd.MenuItemDTO);
            _menuService.Add(modelToAdd.MenuItemDTO);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(string id)
        {
            var menuItem = _menuService.Get(id);
            if (menuItem != null)
            {
                var res = new AddMenuModel
                {
                    MenuItemDTO = menuItem,
                    Categories = _categoryService.GetAll()
                };
                res.MenuItemDTO.Categories.Add(new CategoryDTO());
                return View(res);
            }
            ///TODO:
            ///Change to some Errro view
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(AddMenuModel newDataItem)
        {
            AddImage(newDataItem.MenuItemDTO);
            _menuService.Update(newDataItem.MenuItemDTO);
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
                    char sep = Path.DirectorySeparatorChar;
                    fileName = $@"wwwroot{sep}images{sep}menu{sep}{newFileName}";
                    model.Image = $@"images{sep}menu{sep}" + newFileName;

                    var fullPath = $"{physicalWebRootPath}{sep}{fileName}";
                    using (FileStream fs = System.IO.File.Create(fullPath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
        }
    }
}