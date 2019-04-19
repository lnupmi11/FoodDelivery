using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Models
{
    public class MenuItem
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual ICollection<BasketItem> Baskets { get; set; }
    }
}
