using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories {
    public interface IGenericRepository<TEntity> where TEntity : class {
        Task<IEnumerable<TEntity>> ListAsync();
        Task AddAsync(TEntity entity);
        Task<TEntity> FindByIdAsync(int id);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
