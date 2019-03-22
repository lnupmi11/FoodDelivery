using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.DAL.Models;


namespace FoodDelivery.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {   
        IRepository<MenuItem> MenuItemRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Discount> DiscountRepository { get; }
        IRepository<Basket> BasketRepository { get; }
        void Save();
    }
}