using FoodDelivery.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IMenuService
    {
        MenuItemDTO Get(string id);
        IEnumerable<MenuItemDTO> GetAll();
        void Add(MenuItemDTO menuItem);
        void Update(MenuItemDTO menuItem);
        void Delete(MenuItemDTO menuItem);
        IEnumerable<MenuItemDTO> GetMenuPage(int page, int pagesize, out int pageCount, string sortOpt, string searchWord, string categoryId, string discountId);
    }
}
