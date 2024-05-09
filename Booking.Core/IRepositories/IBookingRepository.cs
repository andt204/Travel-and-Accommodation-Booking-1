using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BookingHotel.Core.IRepositories
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(int id, string userId);
        Task CreateBookingAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task RemoveAsync(int Id);
        Task<Invoice> GetInvoiceByBookingId(int bookingId, string userId);
        Task<IEnumerable<Booking>> GetAllAsync(int pageSize, int pageNumber, string user);
    }
}
