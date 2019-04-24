using FoodDelivery.DAL.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Basket> BasketsRepository { get; }
        IRepository<Discount> DiscountsRepository { get; }
        IRepository<MenuItem> MenuItemsRepository { get; }
        IRepository<Category> CategoriesRepository { get; }
        IRepository<Order> OrdersRepository { get; }
        IRepository<Address> AddressesRepository { get; }
        IRepository<ApplicationUser> UsersRepository { get; }
        IRepository<BasketItem> BasketItemsRepository { get; }
        IRepository<OrderItem> OrderItemsRepository { get; }
        void SaveChanges();
    }
}
