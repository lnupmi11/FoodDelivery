using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace FoodDelivery.BLL.Services
{
    class UserService : IUserService
    {
        private IRepository<ApplicationUser> _userRepository;

        public UserService(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationUser GetApplicationUser(string id)
        {
            return _userRepository.Get(id);
        }

        public IEnumerable<ApplicationUser> GetApplicationUsers()
        {
            return _userRepository.GetAll();
        }

        public void ChangeFirstName(ApplicationUser user, string firstName)
        {
            user.FirstName = firstName;
            _userRepository.Update(user);
        }

        public void ChangeSecondName(ApplicationUser user, string lastName)
        {
            user.LastName = lastName;
            _userRepository.Update(user);
        }

        public void Update(ApplicationUser user)
        {
            _userRepository.Update(user);
        }

        public void Delete(ApplicationUser user)
        {
            //_userRepository.Delete(user);
        }

        public void AddSavedAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.Add(address);
            _userRepository.Update(user);
        }

        public void UpdateSavedAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.ToList().RemoveAll(a => a.Id == address.Id);
            _userRepository.Update(user);
        }

        public void RemoveSaveAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.Remove(address);
            _userRepository.Update(user);
        }
    }
}
