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
        IEnumerable<MenuItemDTO> GetPaginated(int page, int pagesize, string filterOpt, string searchWord);
        int GetCount(string searchWord);
    }
}
