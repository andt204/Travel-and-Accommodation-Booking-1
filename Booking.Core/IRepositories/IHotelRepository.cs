using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace BookingHotel.Core.IRepositories {
    public interface IHotelRepository {
        Task<IEnumerable<Hotel>> ListAsync(int page, int pageSize);
        Task AddAsync(Hotel hotel);
        Task<Hotel> FindByIdAsync(int id);
        void Update(Hotel hotel);
        void Remove(Hotel hotel);
        Task<IEnumerable<Hotel>> SearchAsync(string keyword, int minCapacity, int maxCapacity, int page, int pageSize);
    }
}
