using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Models
{
    public class Basket
    {
        [Key]
        public string Id { get; set; }
        public ICollection<BasketItem> MenuItems { get; set; }
    }
}
