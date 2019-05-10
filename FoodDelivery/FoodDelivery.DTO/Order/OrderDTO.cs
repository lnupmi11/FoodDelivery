using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DTO.Order
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public DateTime SentTime { get; set; }
        public DateTime ReceivedTime { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public double TotalPrice { get; set; }
        public int ItemsCount { get; set; }

        public ApplicationUser User { get; set; }
        public Address Address { get; set; }
        public IEnumerable<OrderItem> OrderItems {get;set;}
        public OrderStatus OrderStatus { get; set; }
    }
}
