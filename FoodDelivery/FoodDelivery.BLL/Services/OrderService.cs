using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.DTO.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.BLL.Services
{
    public class OrderService : IOrderService
    {
        private OrderRepository _orderRepository;
        private MenuItemRepository _menuItemRepository;
        private BasketRepository _basketRepository;

        public OrderService(FoodDeliveryContext context)
        {
            _orderRepository = new OrderRepository(context);
            _menuItemRepository = new MenuItemRepository(context);
            _basketRepository = new BasketRepository(context);
        }

        public void AddItemToBasket(string basketId, string itemId)
        {
            var item = _menuItemRepository.Get(itemId);
            if (item != null)
            {
                var basket = _basketRepository.Get(basketId);
                basket?.MenuItems.Add(new BasketItem { Basket = basket, MenuItem = item, BasketId= basketId, MenuItemId = itemId });
                _basketRepository.SaveChanges();
            }
        }

        public void DeleteItemFromBasket(string basketId, string itemId)
        {
            var item = _menuItemRepository.Get(itemId);
            if (item != null)
            {
                var basket = _basketRepository.Get(basketId);
                var temp = basket?.MenuItems.FirstOrDefault(m => m.MenuItemId == itemId);
                basket?.MenuItems.Remove(temp);
                _basketRepository.SaveChanges();
            }
        }

        public IEnumerable<CartItemDTO> GetAllBasketItems(string basketId)
        {
            var basket = _basketRepository.Get(basketId);
            if (basket != null)
            {
                var menuItemIds = basket.MenuItems.Select(m => m.MenuItemId).ToArray();
                return _menuItemRepository.GetAllWhere(m => menuItemIds.Contains(m.Id)).Select(m => new CartItemDTO
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
