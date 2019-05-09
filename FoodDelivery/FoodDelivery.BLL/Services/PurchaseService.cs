using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
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
                                                   .Include(oi=>oi.MenuItem.Categories)
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
                                                       Categories = oi.MenuItem.Categories.Select(c => new CategoryDTO
                                                       {
                                                           CategoryName = c.CategoryName,
                                                           Description = c.Description,
                                                           Id = c.Id
                                                       }).ToList()
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
                                                           SubmittedTime = oi.FirstOrDefault().Order.SentTime
                                                       }).ToList();
            }
            catch(ArgumentNullException)
            {
                throw new ArgumentException($"There is no user item with the following userName: {userName}");
            }

        }

        public IEnumerable<PurchaseItemDTO> GetPurchaseItemsByFilters(int page, string searchWord, string filterOpt, string categoryId, int itemPerPage, string purchaseId)
        {
            var result = GetPurchaseItems(purchaseId);

            if (!string.IsNullOrEmpty(categoryId))
            {
                result = result.Where(bi => bi.Categories.Any(c => c.Id == categoryId));
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
    }
}
