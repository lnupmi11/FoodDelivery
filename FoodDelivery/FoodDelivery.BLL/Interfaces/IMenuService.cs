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
        IEnumerable<MenuItemDTO> GetPaginated(int currPage, int pageSize);
        void Add(MenuItemDTO menuItem);
        void Update(MenuItemDTO menuItem);
        void Delete(MenuItemDTO menuItem);
        int GetCount();
    }
}
