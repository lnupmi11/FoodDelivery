using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public ICollection<MenuItem> Items { get; set; }
    }
}
