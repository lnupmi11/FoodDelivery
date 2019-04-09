using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FoodDelivery.DAL.Models.Enums;

namespace FoodDelivery.DAL.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public DateTime SentTime {get; set;}
        public DateTime ReceivedTime {get;set;}
        public DateTime EstimatedTime {get; set;}
        public virtual ApplicationUser User { get; set; }
        public virtual Address Address {get;set;}
        public PaymentType PaymentType { get; set;}
    }
}
