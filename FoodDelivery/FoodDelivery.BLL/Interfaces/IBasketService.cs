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
        IEnumerable<CartItemDTO> GetUserBasketByFilters(int page, string searchWord, string filterOpt, string categoryId, string userName, int itemPerPage);
        IEnumerable<CartItemDTO> GetUnAuthorizeUserBasketItems(Dictionary<string, string> userCart, int page, string searchWord, string filterOpt, string categoryId, int itemPerPage);
        void SubmitUnauthorizeUserBasket(Dictionary<string, string> itemsDictionary, string addressId, int paymentType);
        double GetTotalPriceOfUserBasketItems(string userName);
        double GetTotalPriceOfUnAuthorizeUserBasketItems(Dictionary<string, string> userCart);
    }
}
