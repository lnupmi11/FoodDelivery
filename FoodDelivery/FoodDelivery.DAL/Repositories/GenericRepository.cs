using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.DAL.EntityFramework;
using FoodDelivery.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.DAL.Repositories
{
    class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private FoodDeliveryContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(FoodDeliveryContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.FirstOrDefault();
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> condition)
        {
            return _dbSet.Where(condition);
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet;
        }
    }
}