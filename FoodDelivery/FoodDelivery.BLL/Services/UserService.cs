using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using FoodDelivery.DTO;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections;

namespace FoodDelivery.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public ApplicationUser GetApplicationUser(string id)
        {
            return _unitOfWork.UsersRepository.Get(id);
        }

        public IEnumerable<ApplicationUser> GetApplicationUsers()
        {
            return _unitOfWork.UsersRepository.GetAll();
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicatinoUsersByRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return GetApplicationUsers();
            }
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }

        public async Task BlockApplicatoinUser(string id)
        {
            var user = GetApplicationUser(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
        }

        public async Task UnblockApplicationUser(string id)
        {
            var user = GetApplicationUser(id);
            await _userManager.SetLockoutEnabledAsync(user, false);
        }

        public void ChangeFirstName(ApplicationUser user, string firstName)
        {
            user.FirstName = firstName;
            _unitOfWork.UsersRepository.Update(user);
        }

        public void ChangeSecondName(ApplicationUser user, string lastName)
        {
            user.LastName = lastName;
            _unitOfWork.UsersRepository.Update(user);
        }

        public void Update(ApplicationUser user)
        {
            _unitOfWork.UsersRepository.Update(user);
        }

        public void Delete(string id)
        {
            _unitOfWork.UsersRepository.Delete(id);
        }

        public void AddSavedAddress(string userName, AddressDTO address)
        {
            int regionNumber = EnumConverter.GetEnumByDescription<Region>(address.Region, Region.OdesaRegion);
            var user = _unitOfWork.UsersRepository.GetQuery().FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                user.SavedAdresses.Add(new Address
                {
                    BuildingNum = address.BuildingNumber,
                    Region = (Region)regionNumber,
                    Street = address.Street
                });
                _unitOfWork.UsersRepository.Update(user);
            }
        }

        public void AddAddressOfUnauthorizeUser(AddressDTO address)
        {
            int regionNumber = EnumConverter.GetEnumByDescription<Region>(address.Region, Region.OdesaRegion);

            _unitOfWork.AddressesRepository.Create(new Address
            {
                BuildingNum = address.BuildingNumber,
                Region = (Region)regionNumber,
                Street = address.Street
            });
            _unitOfWork.SaveChanges();
        }

        public void UpdateSavedAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.ToList().RemoveAll(a => a.Id == address.Id);
            _unitOfWork.UsersRepository.Update(user);
        }

        public void RemoveSaveAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.Remove(address);
            _unitOfWork.UsersRepository.Update(user);
        }

        public IEnumerable<AddressDTO> GetSavedAddresses(string userName)
        {
            var user = _unitOfWork.UsersRepository.GetQuery()
                                       .Include(u => u.SavedAdresses)
                                       .FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return user.SavedAdresses.Select(a => new AddressDTO
                {
                    BuildingNumber = a.BuildingNum,
                    Region = a.Region.ToString(),
                    Street = a.Street,
                    AddressId = a.Id
                }).ToList();
            }
            else
            {
                throw new ArgumentException($"There is no user item with the following userName: {userName}");
            }
        }

        public string GetSavedAddressId(AddressDTO address)
        {
            int regionId = EnumConverter.GetEnumByDescription<Region>(address.Region, Region.LvivRegion);
            var addressModel = _unitOfWork.AddressesRepository.GetQuery().FirstOrDefault(a =>
                     a.Region == (Region)regionId
                     && a.Street == address.Street 
                     && a.BuildingNum == address.BuildingNumber
                );
            if (addressModel != null)
            {
                return addressModel.Id;
            }
            return null;
        }

        public IEnumerable<string> GetRegions()
        {
            var regionObjs = Enum.GetValues(typeof(Region));
            List<string> regions = new List<string>();
            foreach(var region in regionObjs)
            {
                regions.Add(region.ToString());
            }
            return regions;
        }

        public async Task AssignRoleToUser(string id, string role)
        {
            var user = GetApplicationUser(id);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task RemoveRoleFromUser(string id, string role)
        {
            var user = GetApplicationUser(id);
            await _userManager.RemoveFromRoleAsync(user, role);
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return GetApplicationUsers().FirstOrDefault(u => u.Email == email);
        }
    }
}
