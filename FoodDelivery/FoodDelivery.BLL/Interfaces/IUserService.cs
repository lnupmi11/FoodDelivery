using FoodDelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetApplicationUser(string id);
        IEnumerable<ApplicationUser> GetApplicationUsers();
        void ChangeFirstName(ApplicationUser user, string firstName);
        void ChangeSecondName(ApplicationUser user, string lastName);
        void Update(ApplicationUser user);
        void Delete(ApplicationUser user);
        void AddSavedAddress(ApplicationUser user, Address address);
    }
}
