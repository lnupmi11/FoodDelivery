using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Func<T, bool> predicate);
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        IQueryable<T> GetQuery();
    }
}