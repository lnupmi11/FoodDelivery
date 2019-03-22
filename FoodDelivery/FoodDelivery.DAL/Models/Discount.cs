using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public double Percentage { get; set; }
        public string Description { get; set; }
    }
}
