using FoodDelivery.DTO.Menu;
using System.Collections.Generic;

namespace FoodDelivery.DTO.Cart
{
    public class CartItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
