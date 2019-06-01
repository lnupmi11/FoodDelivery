using FoodDelivery.DTO.Menu;
using FoodDelivery.DTO.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class PurchaseModel
    {
        public IEnumerable<PurchaseItemDTO> PurchaseItems { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public double TotalPrice { get; internal set; }
    }
}
