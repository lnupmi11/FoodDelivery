using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodDelivery.DAL.Repositories
{
    public class BasketRepository
    {
        private readonly FoodDeliveryContext _context;
        private DbSet<Basket> _basketItems;

        public BasketRepository(FoodDeliveryContext context)
        {
            _context = context;
            _basketItems = context.Baskets;
        }

        public IEnumerable<Basket> GetAll()
        {
            return _basketItems
                            .Include(c => c.MenuItems);
        }

        public IEnumerable<Basket> GetAllWhere(Func<Basket, bool> predicate)
        {
            return _basketItems
                            .Include(c => c.MenuItems)
                            .Where(predicate);
        }

        public Basket Get(string id)
        {
            return _basketItems
                            .Include(c => c.MenuItems)
                            .Where(m => m.Id == id)
                            .SingleOrDefault();
        }

        public Basket Get(Func<Basket, bool> predicate)
        {
            return _basketItems
                            .Include(c => c.MenuItems)
                            .Where(predicate)
                            .SingleOrDefault();
        }

        public void Create(Basket basket)
        {
            _basketItems.Add(basket);
            _context.SaveChanges();
        }

        public void Update(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException("Cannot create null Basket");
            }
            _basketItems.Update(basket);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            Basket basket = _basketItems.Find(id);
            if (basket != null)
            {
                _basketItems.Remove(basket);
                _context.SaveChanges();
            }
        }

        public IQueryable<Basket> GetQuery()
        {
            return _basketItems;
        }

        public void SaveChanges()
        {
             _context.SaveChanges();
        }
    }
}
