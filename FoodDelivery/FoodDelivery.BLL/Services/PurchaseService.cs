using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DTO;
using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Purchase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.BLL.Services
{
    public class PurchaseService : IPurchaseService
    {
        public IUnitOfWork _unitOfWork;

        public PurchaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PurchaseItemDTO> GetPurchaseItems(string orderId)
        {
            return _unitOfWork.OrderItemsRepository.GetQuery()
                                                   .Include(oi => oi.MenuItem)
                                                   .Include(oi=>oi.MenuItem.Category)
                                                   .Include(oi => oi.Order)
                                                   .Where(oi => orderId == oi.Order.OrderId)
                                                   .Select(oi => new PurchaseItemDTO
                                                   {
                                                       Id = oi.OrderItemId,
                                                       Count = oi.Count,
                                                       Description = oi.MenuItem.Description,
                                                       Name = oi.MenuItem.Name,
                                                       Price = oi.MenuItem.Price,
                                                       Image = oi.MenuItem.Image,
                                                       Category = oi.MenuItem.Category == null ? null : new CategoryDTO
                                                       {
                                                           CategoryName = oi.MenuItem.Category.CategoryName,
                                                           Description = oi.MenuItem.Category.Description,
                                                           Id = oi.MenuItem.Category.Id
                                                       }
                                                   }).ToList();
        }

        public List<PurchaseDTO> GetListOfPurchases(string userName)
        {
            try
            {
                var orderIds = _unitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.UserName == userName)?
                .Orders
                .Select(o => o.OrderId);

                return _unitOfWork.OrderItemsRepository.GetQuery()
                                                       .Include(oi => oi.MenuItem)
                                                       .Include(oi => oi.Order)
                                                       .Where(oi => orderIds.Contains(oi.Order.OrderId))
                                                       .GroupBy(oi => oi.Order.OrderId)
                                                       .Select(oi => new PurchaseDTO
                                                       {
                                                           Id = oi.Key,
                                                           TotalPrice = oi.Sum(v => v.MenuItem.Price * v.Count),
                                                           SubmittedTime = oi.FirstOrDefault().Order.SentTime,
                                                           Status = oi.FirstOrDefault().Order.Status
                                                       }).ToList();
            }
            catch(ArgumentNullException)
            {
                throw new ArgumentException($"There is no user item with the following userName: {userName}");
            }

        }

        public List<PurchaseDTO> GetFilteredListOfPurchases(string userName, int page, int itemsPerPage, string priceOrder, string orderStatus)
        {
            var result = GetFilteredListOfPurchasesWithoutPage(userName, itemsPerPage,priceOrder, orderStatus);
            result = result.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            return result;
        }

        public List<PurchaseDTO> GetFilteredListOfPurchasesWithoutPage(string userName, int itemsPerPage, string priceOrder, string orderStatus)
        {
            var result = GetListOfPurchases(userName);
            switch (priceOrder)
            {
                case "asc":
                    result = result.OrderBy(r => r.TotalPrice).ToList();
                    break;
                case "desc":
                    result = result.OrderByDescending(r => r.TotalPrice).ToList();
                    break;
            }
            result = result.Where(p => string.IsNullOrEmpty(orderStatus) || p.Status.ToLower().Contains(orderStatus.ToLower())).ToList();
            return result;
        }

        public int GetPurchasesCount(string userName, int itemsPerPage, string priceOrder, string orderStatus)
        {
            return GetFilteredListOfPurchasesWithoutPage(userName, itemsPerPage, priceOrder, orderStatus).Count;
        }

        public IEnumerable<PurchaseItemDTO> GetPurchaseItemsByFilters(FilterMenuItem filter,  string purchaseId)
        {
            var result = GetPurchaseItems(purchaseId);

            if (!string.IsNullOrEmpty(filter.CategoryId))
            {
                result = result.Where(bi => bi.Category.Id == filter.CategoryId);
            }

            if (!string.IsNullOrEmpty(filter.SearchWord))
            {
                result = result.Where(bi => bi.Name.Contains(filter.SearchWord));
            }

            if (filter.FilterOpt == "desc")
            {
                result = result.OrderByDescending(mi => mi.Price);
            }
            else if (filter.FilterOpt == "asc")
            {
                result = result.OrderBy(mi => mi.Price);
            }
            return result.Skip((filter.Page - 1) * filter.ItemPerPage).Take(filter.ItemPerPage);
        }

        public double GetPriceOfPurchaseItems(string purchaseId)
        {
            return GetPurchaseItems(purchaseId).Sum(p => p.Count * p.Price);
        }
    }
}
