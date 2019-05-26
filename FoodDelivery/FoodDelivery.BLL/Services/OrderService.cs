using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Order;
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

        public void Add(OrderDTO order)
        {
            _unitOfWork.OrdersRepository.Create(new DAL.Models.Order
            {
                OrderId = order.OrderId,
                SentTime = order.SentTime,
                ReceivedTime = order.ReceivedTime,
                EstimatedTime = order.EstimatedTime,
                User = order.User,
                Address = order.Address,
                Status = order.OrderStatus.ToString()
            });
            _unitOfWork.SaveChanges();
        }

        public void Delete(OrderDTO order)
        {
            _unitOfWork.OrdersRepository.Delete(order.OrderId);
            _unitOfWork.SaveChanges();
        }

        public OrderDTO Get(string id)
        {
            var order = _unitOfWork.OrdersRepository.GetQuery()
                .Where(o => o.OrderId == id).Include(o => o.User)
                .Include(o => o.Address).FirstOrDefault();

            if (order != null)
            {
                var orderItems = _unitOfWork.OrderItemsRepository.GetQuery()
                    .Include(o => o.Order).Where(o => o.Order.OrderId == id)
                    .Include(o => o.MenuItem);
                double totalPrice = 0;
                int itemsCount = 0;
                if(orderItems != null)
                {
                    totalPrice = orderItems.Sum(o => o.MenuItem.Price);
                    itemsCount = orderItems.Sum(o => o.Count);
                }
                return new OrderDTO
                {
                    OrderId = order.OrderId,
                    SentTime = order.SentTime,
                    ReceivedTime = order.ReceivedTime,
                    EstimatedTime = order.EstimatedTime,
                    User = order.User ?? null,
                    OrderStatus = ValueToEnum(order.Status),
                    OrderItems = orderItems?.AsEnumerable(),
                    TotalPrice = totalPrice,
                    ItemsCount = itemsCount,
                    Address = order.Address ?? null
                };
            }
            return new OrderDTO();
        }

        public IEnumerable<OrderDTO> GetAll()
        {
            var orders = _unitOfWork.OrdersRepository.GetQuery()
                .Include(o => o.User).Include(o => o.Address).OrderBy(o => o.SentTime);
            var orderItems = _unitOfWork.OrderItemsRepository.GetQuery()
                .Include(o => o.Order).Include(o => o.MenuItem);
            double totalPrice = orderItems.Sum(o => o.MenuItem.Price);
            int itemsCount = orderItems.Sum(o => o.Count);

            if (orders != null)
            {
                var result = orders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    SentTime = o.SentTime,
                    ReceivedTime = o.ReceivedTime,
                    EstimatedTime = o.EstimatedTime,
                    User = o.User,
                    OrderStatus = ValueToEnum(o.Status),
                    Address = o.Address,
                    OrderItems = _unitOfWork.OrderItemsRepository.GetQuery().Include(or => or.Order)
                        .Include(or => or.MenuItem).Where(or=> or.Order.OrderId == o.OrderId).AsEnumerable(),
                    TotalPrice = _unitOfWork.OrderItemsRepository.GetQuery().Include(or => or.Order)
                        .Include(or => or.MenuItem).Where(or => or.Order.OrderId == o.OrderId).Sum(or => or.MenuItem.Price),
                    ItemsCount = _unitOfWork.OrderItemsRepository.GetQuery().Include(or => or.Order)
                        .Include(or => or.MenuItem).Where(or => or.Order.OrderId == o.OrderId).Sum(or => or.Count)
                });

                return result;
            }

            return new List<OrderDTO>();
        }

        public IEnumerable<OrderDTO> GetByStatus(string status)
        {
            if(status == String.Empty)
            {
                return GetAll();
            }
            Enum.TryParse(status, out OrderStatus outStatus);
            if(Enum.IsDefined(typeof(OrderStatus), outStatus))
            {
                return GetAll().Where(o => o.OrderStatus == outStatus);
            }
            return GetAll();
        }

        public void Update(OrderDTO order)
        {
            var neworder = _unitOfWork.OrdersRepository.Get(order.OrderId);
            neworder.SentTime = order.SentTime;
            neworder.ReceivedTime = order.ReceivedTime;
            neworder.EstimatedTime = order.EstimatedTime;
            neworder.User = order.User;
            neworder.Status = order.OrderStatus.ToString();
            neworder.Address = order.Address;

            _unitOfWork.OrdersRepository.Update(neworder);
            _unitOfWork.SaveChanges();
        }

        public void UpdateOrderStatus(string id, string statusName)
        {
            var order = _unitOfWork.OrdersRepository.Get(id);
            order.Status = statusName;
            _unitOfWork.OrdersRepository.Update(order);
            _unitOfWork.SaveChanges();
        }

        private OrderStatus ValueToEnum(string value)
        {
            Enum.TryParse(value, out OrderStatus outStatus);
            if (Enum.IsDefined(typeof(OrderStatus), outStatus))
            {
                return outStatus;
            }
            return OrderStatus.Unexpected;
        }
    }
}
