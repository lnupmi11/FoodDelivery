using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;

namespace FoodDelivery.DAL.Repositories
{
    public class UserRepository : IRepository<ApplicationUser>
    {
        private readonly FoodDeliveryContext _context;
        private DbSet<ApplicationUser> _users;

        public UserRepository(FoodDeliveryContext context)
        {
            _context = context;
            _users = context.ApplicationUsers;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _users
                        .Include(a => a.SavedAdresses)
                        .Include(b => b.Basket)
                        .ThenInclude(m => m.MenuItems);
        }

        public IEnumerable<ApplicationUser> GetAllWhere(Func<ApplicationUser, bool> predicate)
        {
            return _users
                        .Include(a => a.SavedAdresses)
                        .Include(b => b.Basket)
                        .ThenInclude(m => m.MenuItems)
                        .Where(predicate);
        }

        public ApplicationUser Get(string id)
        {
            return _users
                        .Include(a => a.SavedAdresses)
                        .Include(b => b.Basket)
                        .ThenInclude(m => m.MenuItems)
                        .SingleOrDefault(u => u.Id == id);
        }
        public ApplicationUser Get(Func<ApplicationUser, bool> predicate)
        {
            return _users
                        .Include(a => a.SavedAdresses)
                        .Include(b => b.Basket)
                        .ThenInclude(m => m.MenuItems)
                        .Where(predicate)
                        .SingleOrDefault();
        }
        public void Create(ApplicationUser user)
        {
            _users.Add(user);
            _context.SaveChanges();
        }
        public void Update(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Trying to create null user");
            }
            _users.Update(user);
            _context.SaveChanges();
        }
        public void Delete(ApplicationUser user)
        {
            if (user == null)
            {
                return;
            }
            _users.Remove(user);
            _context.SaveChanges();
        }
        public void Delete(string id)
        {
            ApplicationUser user = _users.Find(id);
            if(user != null)
            {
                _users.Remove(user);
                _context.SaveChanges();
            }
        }
        public IQueryable<ApplicationUser> GetQuery()
        {
            return _users;
        }
    }
}