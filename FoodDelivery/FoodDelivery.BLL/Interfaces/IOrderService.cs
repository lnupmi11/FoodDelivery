using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        OrderDTO Get(string id);
        IEnumerable<OrderDTO> GetAll();
        IEnumerable<OrderDTO> GetByStatus(string status);
        void Add(OrderDTO order);
        void Update(OrderDTO order);
        void Delete(OrderDTO order);
        void UpdateOrderStatus(string id, string statusName);
    }
}
