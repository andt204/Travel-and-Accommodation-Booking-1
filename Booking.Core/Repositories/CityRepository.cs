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
    public class CityRepository : GenericRepository<City>, ICityRepository {
        public CityRepository(BookingHotelDbContext context) : base(context) {
        }

        public async Task<IEnumerable<City>> ListAsync(int pageSize, int pageNumber) {
            var cities = _context.Cities.AsQueryable();

            cities = _context.Cities
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await cities.ToListAsync();
        }

        public override async Task AddAsync(City city) {
            await _context.Cities.AddAsync(city);
        }

        public override async Task<City> FindByIdAsync(int id) {
            return await _context.Cities.FindAsync(id);
        }

        public override void Update(City city) {
            _context.Cities.Update(city);
        }

        public override void Remove(City city) {
            _context.Cities.Remove(city);
        }
    }
}
