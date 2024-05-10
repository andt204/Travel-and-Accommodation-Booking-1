using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices
{
    public interface IBookingService
    {
        Task<Booking> GetByIdAsync(int UseIid, string userId);
        Task<Booking> CreateBookingAsync(BookingDTO booking, string userId);
        Task UpdateAsync(Booking booking);
        Task RemoveAsync(int Id);
        Task<Invoice> GetInvoiceByBookingId(int bookingId, string userId);
        Task<IEnumerable<Booking>> GetAllAsync(int pageSize, int pageNumber, string userId);
    }
}
