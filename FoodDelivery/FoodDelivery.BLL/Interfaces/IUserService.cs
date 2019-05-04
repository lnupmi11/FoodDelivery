using FoodDelivery.DAL.Models;
using FoodDelivery.DTO;
using System.Collections.Generic;

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
        void AddSavedAddress(string userName, AddressDTO address);
        IEnumerable<AddressDTO> GetSavedAddresses(string userName);
        string GetSavedAddressId(AddressDTO address);
        IEnumerable<string> GetRegions();
    }
}
