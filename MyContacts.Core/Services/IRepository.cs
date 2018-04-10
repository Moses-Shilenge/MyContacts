using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContacts.Core.Services
{
    public interface IRepository<TEntity, in TKey> where TEntity: class
    {
        Task<TEntity> GetByIdAsync(TKey id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}
