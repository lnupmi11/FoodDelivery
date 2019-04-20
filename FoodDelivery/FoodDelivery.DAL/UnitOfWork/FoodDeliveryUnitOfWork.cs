using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDelivery.DAL.UnitOfWork
{
    public class FoodDeliveryUnitOfWork : IUnitOfWork
    {
        private FoodDeliveryContext _context;

        IRepository<Basket> _basketRepository;
        IRepository<Discount> _discountRepository;
        IRepository<MenuItem> _menuItemRepository;
        IRepository<Category> _categoryRepository;
        IRepository<Order> _orderRepository;
        IRepository<Address> _addressRepository;
        IRepository<ApplicationUser> _userRepository;
        IRepository<BasketItem> _basketItemRepository;
        IRepository<OrderItem> _orderItemRepository;

        public FoodDeliveryUnitOfWork(FoodDeliveryContext context)
        {
            _context = context;
        }

        public IRepository<Basket> BasketsRepository {
            get
            {
                if (_basketRepository == null)
                {
                    _basketRepository = new GenericRepository<Basket>(_context);
                }
                return _basketRepository;
            }
        }

        public IRepository<Discount> DiscountsRepository
        {
            get
            {
                if (_discountRepository == null)
                {
                    _discountRepository = new GenericRepository<Discount>(_context);
                }
                return _discountRepository;
            }
        }

        public IRepository<MenuItem> MenuItemsRepository
        {
            get
            {
                if (_menuItemRepository == null)
                {
                    _menuItemRepository = new GenericRepository<MenuItem>(_context);
                }
                return _menuItemRepository;
            }
        }

        public IRepository<Category> CategoriesRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new GenericRepository<Category>(_context);
                }
                return _categoryRepository;
            }
        }

        public IRepository<Order> OrdersRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new GenericRepository<Order>(_context);
                }
                return _orderRepository;
            }
        }

        public IRepository<Address> AddressesRepository
        {
            get
            {
                if (_addressRepository == null)
                {
                    _addressRepository = new GenericRepository<Address>(_context);
                }
                return _addressRepository;
            }
        }

        public IRepository<ApplicationUser> UsersRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<ApplicationUser>(_context);
                }
                return _userRepository;
            }
        }

        public IRepository<BasketItem> BasketItemsRepository
        {
            get
            {
                if (_basketItemRepository == null)
                {
                    _basketItemRepository = new GenericRepository<BasketItem>(_context);
                }
                return _basketItemRepository;
            }
        }

        public IRepository<OrderItem> OrderItemsRepository
        {
            get
            {
                if (_orderItemRepository == null)
                {
                    _orderItemRepository = new GenericRepository<OrderItem>(_context);
                }
                return _orderItemRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
