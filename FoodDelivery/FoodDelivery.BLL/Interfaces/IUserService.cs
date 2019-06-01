using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using FoodDelivery.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IUserService
    {
        ApplicationUser GetApplicationUser(string id);
        ApplicationUser GetUserByEmail(string email);
        IEnumerable<ApplicationUser> GetApplicationUsers();
        Task<IEnumerable<ApplicationUser>> GetApplicatinoUsersByRole(string role);
        Task BlockApplicatoinUser(string id);
        Task UnblockApplicationUser(string id);
        void ChangeFirstName(ApplicationUser user, string firstName);
        void ChangeSecondName(ApplicationUser user, string lastName);
        void Update(ApplicationUser user);
        void Delete(string id);
        Task AssignRoleToUser(string id, string role);
        Task RemoveRoleFromUser(string id, string role);
        void AddSavedAddress(string userName, AddressDTO address);
        IEnumerable<AddressDTO> GetSavedAddresses(string userName);
        string GetSavedAddressId(AddressDTO address);
        IEnumerable<string> GetRegions();
        void AddAddressOfUnauthorizeUser(AddressDTO address);
    }
}
