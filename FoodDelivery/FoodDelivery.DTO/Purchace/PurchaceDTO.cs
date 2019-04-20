using System;
using System.Collections.Generic;

namespace FoodDelivery.DTO.Purchace
{
    public class PurchaceDTO
    {
        public DateTime SubmittedTime { get; set; }
        public string Id { get; set; }
        public List<PurchaceItemDTO> Items { get; set; }
        public double TotalPrice { get; set; }
    }
}
