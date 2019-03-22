using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int ? CategoryId { get; set; }
        public int ? DiscountId { get; set; }
    }
}
