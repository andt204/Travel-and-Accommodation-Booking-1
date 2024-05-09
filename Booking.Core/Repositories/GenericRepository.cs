using BookingHotel.Core.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories {
    public class GenericRepository<TEntity> where TEntity : class {
        internal BookingHotelDbContext _context;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(BookingHotelDbContext context) {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsync(int page, int pageSize) {
            var query = _dbSet.AsQueryable();
            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        public virtual async Task AddAsync(TEntity entity) {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id) {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Update(TEntity entity) {
            _dbSet.Update(entity);
        }

        public virtual void Remove(TEntity entity) {
            _dbSet.Remove(entity);
        }
    }
}
