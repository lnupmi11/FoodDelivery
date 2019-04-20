using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO.Purchace
{
    public class PurchaceItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
    }
}
