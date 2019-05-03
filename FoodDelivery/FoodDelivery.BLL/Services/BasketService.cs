﻿using FoodDelivery.BLL.Interfaces;
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
    public class BasketService : IBasketService
    {
        private IUnitOfWork _unitOfWork;
        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddItemToBasket(string userName, string itemId)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(itemId);
            if (item != null)
            {
                try
                {
                    Basket basket = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                        .Include(u => u.Basket.MenuItems)
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
                catch(NullReferenceException)
                {
                    throw new ArgumentException($"There is no user item with the following userName: {userName}");
                }
            }
            else
            {
                throw new ArgumentException($"There is no menu item with the following id: {itemId}");
            }
        }

        public void DeleteItemFromBasket(string userName, string itemId)
        {
            var item = _unitOfWork.MenuItemsRepository.Get(itemId);
            if (item != null)
            {
                try
                {
                    var basket = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                        .Include(u => u.Basket.MenuItems)
                                                        .FirstOrDefault(u => u.UserName == userName)
                                                        .Basket;
                    if (basket.MenuItems == null)
                    {
                        throw new Exception();
                    }

                    var cartItem = basket?.MenuItems.FirstOrDefault(mi => mi.MenuItemId == itemId);
                    if (cartItem != null)
                    {
                        cartItem.Count--;
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException($"There is no menu item in the user basket with the following id: {itemId}");
                    }
                }
                catch (NullReferenceException)
                {
                    throw new ArgumentException($"There is no user item with the following userName: {userName}");
                }
            }
            else
            {
                throw new ArgumentException($"There is no menu item with the following id: {itemId}");
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
                       Price = menuItem.Price,
                       Image =menuItem.Image
                   };
        }

        public void ClearBasket(string userName)
        {
            try
            {
                var basket = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                    .Include(u => u.Basket.MenuItems)
                                                    .FirstOrDefault(u => u.UserName == userName)
                                                    .Basket;
                if (basket.MenuItems != null)
                {
                    basket.MenuItems.Clear();
                    _unitOfWork.SaveChanges();
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException($"There is no basket with the following user: {userName}");
            }
        }

        public void SubmitBasket(string userName)
        {
            try
            {
                var user = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                    .Include(u => u.Basket.MenuItems)
                                                    .FirstOrDefault(u => u.UserName == userName);
                if (user.Basket.MenuItems != null)
                {
                    List<OrderItem> orderItems = user.Basket.MenuItems.Select(mi => new OrderItem { Count = mi.Count, MenuItem = mi.MenuItem, MenuItemId = mi.MenuItemId }).ToList();
                    _unitOfWork.OrdersRepository.Create(new Order { OrderItems = orderItems, User = user, SentTime= DateTime.Now});
                    ClearBasket(userName);
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException($"There is no basket for user: {userName}");
            }
        }
    }
}
