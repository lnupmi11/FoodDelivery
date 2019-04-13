using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO.Menu;
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
            var item = _unitOfWork.MenuItemsRepository.Get(id);
            if(item != null)
            {
                return new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description
                };
            }
            return null;
        }

        public IEnumerable<MenuItemDTO> GetAll()
        {
            var menu = _unitOfWork.MenuItemsRepository.GetAll();

            if (menu != null)
            {
                return menu.Select(item => new MenuItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price
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
            _unitOfWork.MenuItemsRepository.Update(item);
            _unitOfWork.SaveChanges();
        }

        public void Delete(MenuItemDTO menuItem)
        {
            _unitOfWork.MenuItemsRepository.Delete(menuItem.Id);
            _unitOfWork.SaveChanges();
        }
    }
}
