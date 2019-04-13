using FoodDelivery.DTO.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        void AddItemToBasket(string basketId, string itemId);
        void DeleteItemFromBasket(string basketId, string itemId);
        IEnumerable<CartItemDTO> GetAllUserBasketItems(string basketId);
    }
}
