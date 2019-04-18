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

        public void AddItemToBasket(string userName, string itemId)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(itemId);
            if (item != null)
            {
                var basket = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                        .Include(u=>u.Basket.MenuItems)
                                                        .FirstOrDefault(u => u.UserName == userName)
                                                        .Basket;
                if (basket.MenuItems == null)
                {
                    basket.MenuItems = new List<BasketItem>();
                }

                var cartItem = basket?.MenuItems.FirstOrDefault(mi => mi.MenuItemId == itemId);
                if (cartItem != null)
                {
                    cartItem.Count++;
                }
                else
                {
                    basket?.MenuItems.Add(new BasketItem { Basket = basket, MenuItem = item, BasketId = basket.Id, MenuItemId = itemId, Count = 1 });
                }
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteItemFromBasket(string userName, string itemId)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(itemId);
            if (item != null)
            {
                var basket = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                        .Include(u => u.Basket.MenuItems)
                                                        .FirstOrDefault(u => u.UserName == userName)
                                                        .Basket;
                if (basket.MenuItems == null)
                {
                    return;
                }
                var cartItem = basket?.MenuItems.FirstOrDefault(mi => mi.MenuItemId == itemId);
                if (cartItem != null)
                {
                    cartItem.Count--;
                    _unitOfWork.SaveChanges();
                }            
            }
        }

        public IEnumerable<CartItemDTO> GetAllUserBasketItems(string userName)
        {
            var basketItems = _unitOfWork.UsersRepository.GetQuery()
                                   .Include(u => u.Basket)
                                   .Include(u => u.Basket.MenuItems)
                                   .FirstOrDefault(u => u.UserName == userName)
                                   .Basket
                                   .MenuItems;

            var menuItems = _unitOfWork.MenuItemsRepository.GetQuery();

            return from basketItem in basketItems
                   join menuItem in menuItems on basketItem.MenuItemId equals menuItem.Id
                   select new CartItemDTO {
                       Count = basketItem.Count,
                       Id = menuItem.Id,
                       Description = menuItem.Description,
                       Name = menuItem.Name,
                       Price = menuItem.Price
                   };
        }
    }
}
