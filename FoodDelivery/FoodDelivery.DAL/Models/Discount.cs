using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DAL.Models
{
    public class Discount
    {
        [Key]
        public string Id { get; set; }
        public double Percentage { get; set; }
        public string Description { get; set; }
    }
}
