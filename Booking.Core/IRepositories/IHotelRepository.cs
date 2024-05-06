using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IRepositories {
    public interface IHotelRepository {
        Task<IEnumerable<Hotel>> ListAsync();
        //Task AddAsync(SaveHotelDto hotel);
        Task AddAsync(Hotel hotel);
        Task<Hotel> FindByIdAsync(int id);
        void Update(Hotel hotel);
        Task RemoveAsync(int Id);
    }
}
