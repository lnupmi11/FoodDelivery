using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace FoodDelivery.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApplicationUser GetApplicationUser(string id)
        {
            return _unitOfWork.UsersRepository.Get(id);
        }

        public IEnumerable<ApplicationUser> GetApplicationUsers()
        {
            return _unitOfWork.UsersRepository.GetAll();
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

        public void Delete(ApplicationUser user)
        {
            //_unitOfWork.UsersRepository.Delete(user);
        }

        public void AddSavedAddress(ApplicationUser user, Address address)
        {
            user.SavedAdresses.Add(address);
            _unitOfWork.UsersRepository.Update(user);
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
    }
}
