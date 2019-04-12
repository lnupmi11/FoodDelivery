using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class BasketItem
    {
        public string BasketId { get; set; }
        public Basket Basket { get; set; }

        public string MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
