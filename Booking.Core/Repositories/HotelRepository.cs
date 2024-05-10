using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories {
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository {
        public HotelRepository(BookingHotelDbContext context) : base(context) {
        }

        public override Task AddAsync(Hotel entity) {
            return base.AddAsync(entity);
        }

        public override Task<Hotel> FindByIdAsync(int id) {
            return base.FindByIdAsync(id);
        }

        public override Task<IEnumerable<Hotel>> ListAsync(int page, int pageSize) {
            return base.ListAsync(page, pageSize);
        }

        public override void Remove(Hotel entity) {
            base.Remove(entity);
        }
        public override void Update(Hotel entity) {
            base.Update(entity);
        }
       
        public async Task<IEnumerable<Hotel>> SearchAsync(string keyword, int minCapacity, int maxCapacity, int page, int pageSize) {
            var query = _context.Hotels.Where(h => h.Name.Contains(keyword) || h.Description.Contains(keyword) || h.Address.Contains(keyword));

            if (minCapacity > 0)
                query = query.Where(h => h.NumOfRoom >= minCapacity);

            if (maxCapacity > 0)
                query = query.Where(h => h.NumOfRoom <= maxCapacity);

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

       
    }
}
