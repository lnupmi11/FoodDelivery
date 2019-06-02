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
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public MenuItemDTO Get(string id)
        {
            var item = _unitOfWork.MenuItemsRepository.GetQuery().Where(i => i.Id == id)
                .Include(i => i.Discount).Include(i => i.Category).FirstOrDefault();
            if(item != null)
            {
                var res = new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    Image = item.Image,
                    Category = item.Category == null ? null : new CategoryDTO
                    {
                        Id = item.Category.Id,
                        CategoryName = item.Category.CategoryName,
                        Description = item.Category.Description
                    },
                    Discount = item.Discount == null ? null : new DiscountDTO
                    {
                        Id = item.Discount.Id,
                        Percentage = item.Discount.Percentage,
                        Description = item.Discount.Description
                    }
                };
                return res;
            }
            return null;
        }

        public IEnumerable<MenuItemDTO> GetAll()
        {
            var menu = _unitOfWork.MenuItemsRepository.GetQuery()
                .Include(i => i.Discount).Include(i => i.Category).Where(mi=>mi.IsActive.Value);

            if (menu != null)
            {
                return menu.Select(item => new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Image = item.Image,
                    Category = item.Category == null ? null : new CategoryDTO
                    {
                        Id = item.Category.Id,
                        CategoryName = item.Category.CategoryName,
                        Description = item.Category.Description
                    },
                    Discount = item.Discount == null ? null : new DiscountDTO
                    {
                        Id = item.Discount.Id,
                        Percentage = item.Discount.Percentage,
                        Description = item.Discount.Description
                    }
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
                Category = menuItem.Category == null ? null : _unitOfWork.CategoriesRepository.Get(menuItem.Category.Id),
                Discount = menuItem.Discount == null ? null : _unitOfWork.DiscountsRepository.Get(menuItem.Discount.Id),
                IsActive = true

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
            item.Category = menuItem.Category == null ? null : _unitOfWork.CategoriesRepository.Get(menuItem.Category.Id);
            item.Discount = menuItem.Discount == null ? null : _unitOfWork.DiscountsRepository.Get(menuItem.Discount.Id);
            if (!string.IsNullOrEmpty(menuItem.Image))
                item.Image = menuItem.Image;
            _unitOfWork.MenuItemsRepository.Update(item);
            _unitOfWork.SaveChanges();
        }

        public void Delete(MenuItemDTO menuItem)
        {
            var result = _unitOfWork.MenuItemsRepository.Get(menuItem.Id);
            result.IsActive = false;
            _unitOfWork.MenuItemsRepository.Update(result);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<MenuItemDTO> GetMenuPage(int page, int pageSize, out int pageCount, string filterOpt, string searchWord, string categoryId, string discountId)
        {
            var result = GetAll();
            if(!string.IsNullOrEmpty(categoryId))
            {
                result = result.Where(i =>
                {
                    bool v = i.Category != null && i.Category.Id == categoryId;
                    return v;
                });
            }
            if (!string.IsNullOrEmpty(discountId))
            {
                result = result.Where(i =>
                {
                    bool v = i.Discount != null && i.Discount.Id == discountId;
                    return v;
                });
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                result = result.Where(i => i.Name.ToLower().Contains(searchWord.ToLower()));
            }
            pageCount = (int)Math.Ceiling((double)result.Count() / pageSize);
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
    }
}
