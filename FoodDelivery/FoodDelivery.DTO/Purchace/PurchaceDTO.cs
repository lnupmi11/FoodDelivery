using System;
using System.Collections.Generic;

namespace FoodDelivery.DTO.Purchase
{
    public class PurchaseDTO
    {
        public int PaymentType { get; set; }
        public AddressDTO Address { get; set; }
        public DateTime SubmittedTime { get; set; }
        public string Id { get; set; }
        public List<PurchaseItemDTO> Items { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
