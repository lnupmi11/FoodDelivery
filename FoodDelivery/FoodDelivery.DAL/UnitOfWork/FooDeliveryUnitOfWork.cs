using System;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Repositories;

namespace FoodDelivery.DAL.UnitOfWork
{
    public class FoodDeliveryUnitOfWork
    {
        private FoodDeliveryContext db = new FoodDeliveryContext();
        private IRepository<Basket> _basketsRepository;
        private IRepository<Discount> _discountRepository;
        private IRepository<Category> _itemCategoryRepository;
        private IRepository<MenuItem> _itemRepository;

        public IRepository<Basket> BasketRepository
        {
            get
            {
                if (_basketsRepository == null)
                    _basketsRepository = new GenericRepository<Basket>(db);
                return _basketsRepository;
            }
        }

        public IRepository<Discount> DiscountRepository
        {
            get
            {
                if (_discountRepository == null)
                    _discountRepository = new GenericRepository<Discount>(db);
                return _discountRepository;
            }
        }

        public IRepository<Category> ItemCategoryRepository
        {
            get
            {
                if (_itemCategoryRepository == null)
                    _itemCategoryRepository = new GenericRepository<Category>(db);
                return _itemCategoryRepository;
            }
        }

        public IRepository<MenuItem> ItemRepository
        {
            get
            {
                if (_itemRepository == null)
                    _itemRepository = new GenericRepository<MenuItem>(db);
                return _itemRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
