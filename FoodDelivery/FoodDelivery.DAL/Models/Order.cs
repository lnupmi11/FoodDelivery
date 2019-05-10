using System.ComponentModel.DataAnnotations;
using System;
using FoodDelivery.DAL.Models.Enums;
using System.Collections.Generic;

namespace FoodDelivery.DAL.Models
{
    public class Order
    {
        public string OrderId { get; set; }

        public DateTime SentTime {get; set;}
        public DateTime ReceivedTime {get;set;}
        public TimeSpan EstimatedTime {get; set;}
        public string Status { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual Address Address {get;set;}

        public PaymentType PaymentType { get; set;}
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
