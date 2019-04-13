using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;

namespace FoodDelivery.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly FoodDeliveryContext _context;
        private DbSet<Order> _orders;

        public OrderRepository(FoodDeliveryContext context)
        {
            _context = context;
            _orders = context.Orders;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders
                            .Include(u => u.User)
                            .ThenInclude(b => b.Basket)
                            .ThenInclude(m => m.MenuItems)
                            .Include(a => a.Address);
        }

        public IEnumerable<Order> GetAllWhere(Func<Order, bool> predicate)
        {
            return _orders
                            .Include(u => u.User)
                            .ThenInclude(b => b.Basket)
                            .ThenInclude(m => m.MenuItems)
                            .Include(a => a.Address)
                            .Where(predicate);
        }

        public Order Get(string id)
        {
            return _orders
                            .Include(u => u.User)
                            .ThenInclude(b => b.Basket)
                            .ThenInclude(m => m.MenuItems)
                            .Include(a => a.Address)
                            .Where(m => m.Id == id)
                            .SingleOrDefault();            
        }

        public Order Get(Func<Order, bool> predicate)
        {
            return _orders
                            .Include(u => u.User)
                            .ThenInclude(b => b.Basket)
                            .ThenInclude(m => m.MenuItems)
                            .Include(a => a.Address)
                            .Where(predicate)
                            .SingleOrDefault();       
        }

        public void Create(Order Order)
        {
            _orders.Add(Order);
            _context.SaveChanges();
        }

        public void Update(Order Order)
        {
            if(Order == null)
            {
                throw new ArgumentNullException("Cannot create null Order");
            }
            _orders.Update(Order);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            Order Order = _orders.Find(id);
            if( Order != null)
            {
                _orders.Remove(Order);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}