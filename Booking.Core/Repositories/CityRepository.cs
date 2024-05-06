using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories {
    public class CityRepository : BaseRepository, ICityRepository {
        public CityRepository(BookingHotelDbContext context) : base(context) {
        }

        public async Task<IEnumerable<City>> ListAsync() {
            return await _context.Cities.ToListAsync();
        }

        public async Task AddAsync(City city) {
            await _context.Cities.AddAsync(city);
        }

        public async Task<City> FindByIdAsync(int id) {
            return await _context.Cities.FindAsync(id);
        }

        public void Update(City city) {
            _context.Cities.Update(city);
        }

        public void Remove(City city) {
            _context.Cities.Remove(city);
        }
    }
}
