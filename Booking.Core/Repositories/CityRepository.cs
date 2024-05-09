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

        public override async Task AddAsync(City entity) {
             await base.AddAsync(entity);
        }

        public override async Task<City> FindByIdAsync(int id) {
            return await base.FindByIdAsync(id);
        }

        public override async Task<IEnumerable<City>> ListAsync(int page, int pageSize) {
            return await base.ListAsync(page, pageSize);
        }

        public override void Remove(City entity) {
            base.Remove(entity);
        }

        public override void Update(City entity) {
            base.Update(entity);
        }

        public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int count, int page = 1, int pageSize = 5) {
            // Calculate the number of items to skip based on the page and pageSize
            var skipAmount = (page - 1) * pageSize;

            // Retrieve the top N visited cities
            var topVisitedCities = await _context.Cities
                .OrderByDescending(c => c.VisitedCount)
                .Take(count)
                .ToListAsync();

            // Apply pagination to the result set
            var paginatedCities = topVisitedCities
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            return paginatedCities;
        }


    }
}
