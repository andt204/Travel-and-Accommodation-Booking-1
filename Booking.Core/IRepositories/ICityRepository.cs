using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IRepositories {
    public interface ICityRepository {
        Task<IEnumerable<City>> ListAsync(int pageSize, int pageNumber);
        Task AddAsync(City city);
        Task<City> FindByIdAsync(int id);
        void Update(City city);
        void Remove(City city);
    }
}
