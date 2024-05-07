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

        public override async Task<IEnumerable<Hotel>> ListAsync() {
            return await _context.Hotels.ToListAsync();
        }

        public override async Task AddAsync(Hotel hotel) {
            await _context.Hotels.AddAsync(hotel);
        }

        public override async Task<Hotel> FindByIdAsync(int id) {
            return await _context.Hotels.FindAsync(id);
        }

        public override void Update(Hotel hotel) {
            _context.Hotels.Update(hotel);
        }

        public override void Remove(Hotel hotel) {
            _context.Hotels.Remove(hotel);
        }
        public async Task<IEnumerable<Hotel>> SearchAsync(string keyword, int minCapacity, int maxCapacity) {

            var query = _context.Hotels.Where(h => h.Name.Contains(keyword) || h.Description.Contains(keyword) || h.Address.Contains(keyword));

            if (minCapacity > 0)
                query = query.Where(h => h.NumOfRoom >= minCapacity);

            if (maxCapacity > 0)
                query = query.Where(h => h.NumOfRoom <= maxCapacity);

            return await query.ToListAsync();
        }
    }
}
