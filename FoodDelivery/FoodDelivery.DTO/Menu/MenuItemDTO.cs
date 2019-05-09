using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO.Menu
{
    public class MenuItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public CategoryDTO Category { get; set; }
        public DiscountDTO Discount { get; set; }
    }
}
