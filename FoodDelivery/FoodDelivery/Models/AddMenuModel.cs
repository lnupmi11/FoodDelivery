using FoodDelivery.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class AddMenuModel
    {
        public MenuItemDTO MenuItemDTO { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public IEnumerable<DiscountDTO> Discounts { get; set; }
    }
}
