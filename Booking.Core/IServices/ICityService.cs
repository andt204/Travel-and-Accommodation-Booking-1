using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices {
    public interface ICityService {
        Task<IEnumerable<City>> ListAsync(int pageSize, int pageNumber);
        Task<CityResponse> SaveAsync(City city);
        Task<CityResponse> UpdateAsync(int id, City city);
        Task<CityResponse> DeleteAsync(int id);
    }
}
