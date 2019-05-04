using FoodDelivery.DTO.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IBasketService
    {
        void AddItemToBasket(string basketId, string itemId);
        void DeleteItemFromBasket(string basketId, string itemId);
        void ClearBasket(string basketId);
        void SubmitBasket(string basketId, string addressId, int paymentType);
        IEnumerable<CartItemDTO> GetAllUserBasketItems(string basketId);
    }
}
