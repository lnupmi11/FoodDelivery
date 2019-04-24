using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO.Menu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.BLL.Services
{
    public class MenuService : IMenuService
    {
        private IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public MenuItemDTO Get(string id)
        {
            var item = _unitOfWork.MenuItemsRepository.GetQuery().Where(i => i.Id == id)
                .Include(i => i.Discounts).Include(i => i.Categories).FirstOrDefault();
            if(item != null)
            {
                var res = new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    Image = item.Image,
                    Categories = item.Categories.Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Description = c.Description
                    }).ToList()
                };
                return res;
            }
            return null;
        }

        public IEnumerable<MenuItemDTO> GetAll()
        {
            var menu = _unitOfWork.MenuItemsRepository.GetQuery()
                .Include(i => i.Discounts).Include(i => i.Categories);

            if (menu != null)
            {
                return menu.Select(item => new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Image = item.Image,
                    Categories = item.Categories.Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Description = c.Description
                    }).ToList()
                });

            }
            return new List<MenuItemDTO>();
        }

        public void Add(MenuItemDTO menuItem)
        {
            _unitOfWork.MenuItemsRepository.Create(new MenuItem
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Description = menuItem.Description,
                Image = menuItem.Image,
                Categories = _unitOfWork.CategoriesRepository.GetAllWhere(c => menuItem.Categories.Any(x => x.Id == c.Id)).ToList()
            });
            _unitOfWork.SaveChanges();
        }

        public void Update(MenuItemDTO menuItem)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(menuItem.Id);
            item.Id = menuItem.Id;
            item.Name = menuItem.Name;
            item.Price = menuItem.Price;
            item.Description = menuItem.Description;
            item.Categories = _unitOfWork.CategoriesRepository.GetAllWhere(c => menuItem.Categories.Any(x => x.Id == c.Id)).ToList();
            if (menuItem.Image != null && menuItem.Image != String.Empty)
                item.Image = menuItem.Image;
            _unitOfWork.MenuItemsRepository.Update(item);
            _unitOfWork.SaveChanges();
        }

        public void Delete(MenuItemDTO menuItem)
        {
            _unitOfWork.MenuItemsRepository.Delete(menuItem.Id);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<MenuItemDTO> GetPaginated(int page, int pageSize, string filterOpt, string searchWord, string categoryId)
        {
            var result = GetAll();
            if(categoryId != null && categoryId != String.Empty)
            {
                result = result.Where(c => c.Categories.Any(x => x.Id == categoryId));
            }
            if(searchWord != null && searchWord != String.Empty)
            {
                result = result.Where(i => i.Name.Contains(searchWord));
            }
            switch (filterOpt)
            {
                case "asc":
                    result = result.OrderBy(i => i.Price);
                    break;
                case "desc":
                    result = result.OrderByDescending(i => i.Price);
                    break;
            }
            result = result.Skip((page - 1) * pageSize).Take(pageSize);
            return result;
        }

        public int GetCount(string searchWord)
        {
            if(searchWord != null && searchWord != String.Empty)
            {
                return _unitOfWork.MenuItemsRepository.GetAll().Where(i => i.Name.Contains(searchWord)).Count();
            }
            return _unitOfWork.MenuItemsRepository.GetAll().Count();
        }
    }
}
