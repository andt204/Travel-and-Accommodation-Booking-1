using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices {
    public interface IHotelService {
        Task<IEnumerable<Hotel>> ListAsync();
        Task<HotelResponse> SaveAsync(Hotel hotel);
        Task<HotelResponse> UpdateAsync(int id, Hotel hotel);
        Task<HotelResponse> DeleteAsync(int id);
        Task<HotelResponse> FindByIdAsync(int id);
        Task<IEnumerable<Hotel>> SearchAsync(string keyword = null, int? minCapacity = null, int? maxCapacity = null);
    }
}
