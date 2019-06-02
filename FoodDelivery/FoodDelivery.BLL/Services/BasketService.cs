using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Cart;
using FoodDelivery.DTO.Menu;
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
                    if (cartItem != null && cartItem.Count > 1)
                    {
                        cartItem.Count--;
                        _unitOfWork.SaveChanges();
                    }
                    else if(cartItem != null && cartItem.Count == 1)
                    {
                        basket?.MenuItems.Remove(cartItem);
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
            try
            {
                var basketItems = _unitOfWork.UsersRepository.GetQuery()
                                       .Include(u => u.Basket)
                                       .Include(u => u.Basket.MenuItems)
                                       .FirstOrDefault(u => u.UserName == userName)
                                       .Basket
                                       .MenuItems;
                var menuItems = _unitOfWork.MenuItemsRepository.GetQuery().Include(mi => mi.Category).ToList();

                return from basketItem in basketItems
                       join menuItem in menuItems on basketItem.MenuItemId equals menuItem.Id
                       select new CartItemDTO
                       {
                           Count = basketItem.Count,
                           Id = menuItem.Id,
                           Description = menuItem.Description,
                           Name = menuItem.Name,
                           Price = menuItem.Price,
                           Image = menuItem.Image,
                           Category = menuItem.Category == null ? null : new CategoryDTO
                           {
                               CategoryName = menuItem.Category.CategoryName,
                               Description = menuItem.Category.Description,
                               Id = menuItem.Category.Id
                           }
                       };
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException($"There is no user item with the following userName: {userName}");
            }
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

        public void SubmitBasket(string userName, string addresId, int paymentType)
        {
            try
            {
                var user = _unitOfWork.UsersRepository.GetQuery().Include(u => u.Basket)
                                                    .Include(u => u.Basket.MenuItems)
                                                    .FirstOrDefault(u => u.UserName == userName);
                if (user.Basket.MenuItems != null)
                {
                    List<OrderItem> orderItems = user.Basket.MenuItems.Select(mi => new OrderItem
                    {
                        Count = mi.Count,
                        MenuItem = mi.MenuItem,
                        MenuItemId = mi.MenuItemId,
                    }).ToList();
                    var address = _unitOfWork.AddressesRepository.GetQuery().FirstOrDefault(a => a.Id == addresId);
                    if (address != null)
                    {
                        var paymentTypeEnum = (PaymentType)paymentType;
                        _unitOfWork.OrdersRepository.Create(new Order {
                            OrderItems = orderItems,
                            User = user,
                            SentTime = DateTime.Now,
                            Address = address,
                            PaymentType = paymentTypeEnum,
                            Status = OrderStatus.WaitingResponse.ToString()
                        });
                        ClearBasket(userName);
                    }
                }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException($"There is no basket for user: {userName}");
            }
        }

        public IEnumerable<CartItemDTO> GetUserBasketByFilters(int page, string searchWord, string filterOpt, string categoryId, string userName, int itemPerPage)
        {
            var result = GetAllUserBasketItems(userName);

            if (!string.IsNullOrEmpty(categoryId))
            {
                result = result.Where(bi => bi.Category.Id == categoryId);
            }

            if (!string.IsNullOrEmpty(searchWord))
            {
                result = result.Where(bi => bi.Name.Contains(searchWord));
            }

            if (filterOpt == "desc")
            {
                result = result.OrderByDescending(mi => mi.Price);
            }
            else if (filterOpt == "asc")
            {
                result = result.OrderBy(mi => mi.Price);
            }

            return result.Skip((page - 1) * itemPerPage).Take(itemPerPage);
        }

        public IEnumerable<CartItemDTO> GetUnAuthorizeUserBasketItems(Dictionary<string,string> userCart, int page, string searchWord, string filterOpt, string categoryId, int itemPerPage)
        {
            try
            {
                var result = GetAllUnauthorizeUserBasketItems(userCart);

                if (!string.IsNullOrEmpty(categoryId))
                {
                    result = result.Where(bi => bi.Category.Id == categoryId);
                }

                if (!string.IsNullOrEmpty(searchWord))
                {
                    result = result.Where(bi => bi.Name.Contains(searchWord));
                }

                if (filterOpt == "desc")
                {
                    result = result.OrderByDescending(mi => mi.Price);
                }
                else if (filterOpt == "asc")
                {
                    result = result.OrderBy(mi => mi.Price);
                }

                return result.Skip((page - 1) * itemPerPage).Take(itemPerPage);
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException($"UserCart is null");
            }
        }


        public void SubmitUnauthorizeUserBasket(Dictionary<string, string> itemsInCart, string addressId, int paymentType)
        {

            var menuItems = _unitOfWork.MenuItemsRepository.GetAll();

            List<OrderItem> orderItems = (from menuItem in menuItems
                                          join item in itemsInCart on menuItem.Id equals item.Key
                                          select new OrderItem
                                          {
                                              Count = int.Parse(item.Value),
                                              MenuItem = menuItem,
                                              MenuItemId = menuItem.Id,
                                          }).ToList();

            var address = _unitOfWork.AddressesRepository.GetQuery().FirstOrDefault(a => a.Id == addressId);
            if (address != null)
            {
                var paymentTypeEnum = (PaymentType)paymentType;
                _unitOfWork.OrdersRepository.Create(new Order { OrderItems = orderItems, User = null, SentTime = DateTime.Now, Address = address, PaymentType = paymentTypeEnum });
            }
        }

        public double GetTotalPriceOfUnAuthorizeUserBasketItems(Dictionary<string, string> userCart)
        {
            var result = GetAllUnauthorizeUserBasketItems(userCart);
            return result.Sum(i => i.Price * i.Count);
        }

        public IEnumerable<CartItemDTO> GetAllUnauthorizeUserBasketItems(Dictionary<string, string> userCart)
        {
            var menuItems = _unitOfWork.MenuItemsRepository.GetQuery().Include(mi => mi.Category).ToList();
            var result = from item in userCart
                         join menuItem in menuItems on item.Key equals menuItem.Id
                         select new CartItemDTO
                         {
                             Count = int.Parse(item.Value),
                             Id = menuItem.Id,
                             Description = menuItem.Description,
                             Name = menuItem.Name,
                             Price = menuItem.Price,
                             Image = menuItem.Image,
                             Category = menuItem.Category == null ? null : new CategoryDTO
                             {
                                 CategoryName = menuItem.Category.CategoryName,
                                 Description = menuItem.Category.Description,
                                 Id = menuItem.Category.Id
                             }
                         };
            return result;
        }

        public double GetTotalPriceOfUserBasketItems(string userName)
        {
            var result = GetAllUserBasketItems(userName);
            return result.Sum(i => i.Count * i.Price);
        }
    }
}
