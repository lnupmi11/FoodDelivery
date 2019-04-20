using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public string MenuItemId { get; set; }
        public int Count { get; set; }
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
