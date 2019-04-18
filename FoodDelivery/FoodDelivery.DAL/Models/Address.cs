using System.ComponentModel.DataAnnotations;
using FoodDelivery.DAL.Models.Enums;

namespace FoodDelivery.DAL.Models
{
    public class Address
    {
        [Key]
        public string Id { get; set; }
        public Region Region { get; set; }
        public string Street { get; set; }
        public int BuildingNum { get; set; }
    }
}
