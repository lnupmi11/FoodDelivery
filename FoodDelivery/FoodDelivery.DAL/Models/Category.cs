using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
