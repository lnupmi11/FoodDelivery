using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Menu;
using System.Collections.Generic;


namespace FoodDelivery.Models
{
    public class CartModel
    {
        public IEnumerable<CartItemDTO> CartItems { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public double TotalPrice { get; internal set; }
    }
}
