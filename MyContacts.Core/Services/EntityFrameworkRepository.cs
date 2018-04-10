using Microsoft.EntityFrameworkCore;
using MyContacts.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyContacts.Core.Services
{
    public class EntityFrameworkRepository<TEntity, TKey> where TEntity : class
    {
        private MyContactsContext _context;

        public EntityFrameworkRepository(MyContactsContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            return await Task.FromResult(query);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }        

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entity;
            
        }

        //// Might possibly use
        //public async Task<IQueryable<TEntity>> IncludeMultiple<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        //{
        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query,
        //                  (current, include) => current.Include(include));
        //    }

        //    return await Task.FromResult(query);
        //}
    }
}
