using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;

namespace FoodDelivery.DAL.Repositories
{
    public class MenuItemRepository : IRepository<MenuItem>
    {
        private readonly FoodDeliveryContext _context;
        private DbSet<MenuItem> _menuItems;

        public MenuItemRepository(FoodDeliveryContext context)
        {
            _context = context;
            _menuItems = context.MenuItems;
        }

        public IEnumerable<MenuItem> GetAll()
        {
            return _menuItems
                            .Include(c => c.Categories)
                            .Include(d => d.Discounts);
        }

        public IEnumerable<MenuItem> GetAllWhere(Func<MenuItem, bool> predicate)
        {
            return _menuItems
                            .Include(c => c.Categories)
                            .Include(d => d.Discounts)
                            .Where(predicate);
        }

        public MenuItem Get(string id)
        {
            return _menuItems
                            .Include(c => c.Categories)
                            .Include(d => d.Discounts)
                            .Where(m => m.Id == id)
                            .SingleOrDefault();            
        }

        public MenuItem Get(Func<MenuItem, bool> predicate)
        {
            return _menuItems
                            .Include(c => c.Categories)
                            .Include(d => d.Discounts)
                            .Where(predicate)
                            .SingleOrDefault();       
        }

        public void Create(MenuItem menuItem)
        {
            _menuItems.Add(menuItem);
            _context.SaveChanges();
        }

        public void Update(MenuItem menuItem)
        {
            if(menuItem == null)
            {
                throw new ArgumentNullException("Cannot create null MenuItem");
            }
            _menuItems.Update(menuItem);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            MenuItem menuItem = _menuItems.Find(id);
            if( menuItem != null)
            {
                _menuItems.Remove(menuItem);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}