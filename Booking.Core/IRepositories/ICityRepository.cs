using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IRepositories {
    public interface ICityRepository {
        Task<IEnumerable<City>> ListAsync(int page, int pageSize);
        Task AddAsync(City city);
        Task<City> FindByIdAsync(int id);
        void Update(City city);
        void Remove(City city);
        Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int count, int page = 1, int pageSize = 5);
    }
}
