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
            var item = _unitOfWork.MenuItemsRepository.GetQuery()
                .Include(i => i.Discounts).Where(i => i.Id == id).FirstOrDefault();
            if(item != null)
            {
                var res = new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    Image = item.Image,
                };
            }
            return null;
        }

        public IEnumerable<MenuItemDTO> GetAll()
        {
            var menu = _unitOfWork.MenuItemsRepository.GetQuery()
                .Include(i => i.Discounts);

            if (menu != null)
            {
                return menu.Select(item => new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Image = item.Image
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
                Image = menuItem.Image
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
            if(menuItem.Image != String.Empty)
                item.Image = menuItem.Image;
            _unitOfWork.MenuItemsRepository.Update(item);
            _unitOfWork.SaveChanges();
        }

        public void Delete(MenuItemDTO menuItem)
        {
            _unitOfWork.MenuItemsRepository.Delete(menuItem.Id);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<MenuItemDTO> GetPaginated(int page, int pageSize, string filterOpt, string searchWord)
        {
            var result = GetAll();
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
            return _unitOfWork.MenuItemsRepository.GetQuery().Where(i => i.Name.Contains(searchWord)).Count();
        }
    }
}
