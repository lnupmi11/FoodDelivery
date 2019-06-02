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
        public bool? IsActive { get; set; }
        public virtual Category Category { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual ICollection<BasketItem> Baskets { get; set; }

        public ICollection<AdditionalInfoAboutMenuItem> AdditionalInfoAboutMenuItems { get; set; }
    }
}
