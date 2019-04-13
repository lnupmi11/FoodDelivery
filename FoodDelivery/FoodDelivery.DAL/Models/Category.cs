using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
