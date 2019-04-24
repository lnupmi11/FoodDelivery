using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DTO.Purchace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.BLL.Services
{
    public class PurchaceService : IPurchaceService
    {
        public IUnitOfWork _unitOfWork;

        public PurchaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<PurchaceItemDTO> GetPurchaceItems(string orderId)
        {
            return _unitOfWork.OrderItemsRepository.GetQuery()
                                                   .Include(oi => oi.MenuItem)
                                                   .Include(oi => oi.Order)
                                                   .Where(oi => orderId == oi.Order.OrderId)
                                                   .Select(oi => new PurchaceItemDTO
                                                   {
                                                       Id = oi.OrderItemId,
                                                       Count = oi.Count,
                                                       Description = oi.MenuItem.Description,
                                                       Name = oi.MenuItem.Name,
                                                       Price = oi.MenuItem.Price,
                                                       Image = oi.MenuItem.Image
                                                   }).ToList();
        }

        public List<PurchaceDTO> GetListOfPurchaces(string userName)
        {
            var orderIds = _unitOfWork.UsersRepository.GetQuery()
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.UserName == userName)
                .Orders
                .Select(o => o.OrderId);

            return _unitOfWork.OrderItemsRepository.GetQuery()
                                                   .Include(oi => oi.MenuItem)
                                                   .Include(oi => oi.Order)
                                                   .Where(oi => orderIds.Contains(oi.Order.OrderId))
                                                   .GroupBy(oi => oi.Order.OrderId)
                                                   .Select(oi => new PurchaceDTO
                                                   {
                                                       Id = oi.Key,
                                                       TotalPrice = oi.Sum(v=> v.MenuItem.Price * v.Count),
                                                       SubmittedTime = oi.FirstOrDefault().Order.SentTime
                                                   }).ToList();

        }
    }
}
