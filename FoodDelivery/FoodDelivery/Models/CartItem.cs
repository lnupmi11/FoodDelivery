using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.DTO.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public string ItemTitle { get; set; }
        public string Discription { get; set; }
        public double Price { get; set; }
    }
}
