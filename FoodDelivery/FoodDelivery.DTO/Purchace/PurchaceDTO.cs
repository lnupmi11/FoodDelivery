using System;
using System.Collections.Generic;

namespace FoodDelivery.DTO.Purchase
{
    public class PurchaseDTO
    {
        public DateTime SubmittedTime { get; set; }
        public string Id { get; set; }
        public List<PurchaseItemDTO> Items { get; set; }
        public double TotalPrice { get; set; }
    }
}
