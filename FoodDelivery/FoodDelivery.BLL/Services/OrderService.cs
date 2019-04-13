using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DTO.Cart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddItemToBasket(string basketId, string itemId)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(itemId);
            if (item != null)
            {
                var basket = _unitOfWork.BasketsRepository.Get(basketId);
                basket?.MenuItems.Add(new BasketItem { Basket = basket, MenuItem = item, BasketId= basketId, MenuItemId = itemId });
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteItemFromBasket(string basketId, string itemId)
        {

        }

        public IEnumerable<CartItemDTO> GetAllBasketItems(string basketId)
        {
            var basket = _unitOfWork.BasketsRepository.GetQuery().Include(b=>b.MenuItems).FirstOrDefault(b=>b.Id == basketId);
            if (basket != null)
            {
                var menuItemIds = basket.MenuItems.Select(m => m.MenuItemId).ToArray();
                return _unitOfWork.MenuItemsRepository.GetAllWhere(m => menuItemIds.Contains(m.Id)).Select(m => new CartItemDTO
                {
                    Description = m.Description,
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price
                });
            }
            return new List<CartItemDTO>();
        }
    }
}
