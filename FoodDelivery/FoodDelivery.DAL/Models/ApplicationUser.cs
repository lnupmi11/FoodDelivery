using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FoodDelivery.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Basket = new Basket();
            SavedAdresses = new List<Address>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Address> SavedAdresses { get; set; }
        public DateTime RegistrationDate {get;set;}
        public virtual Basket Basket { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
