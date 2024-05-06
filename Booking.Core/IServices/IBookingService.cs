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
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(int id);
        Task CreateBooking(BookingDTO booking);
        Task UpdateAsync(Booking booking);
        Task RemoveAsync(int Id);
        Task<Invoice> GetInvoiceByBookingId(int bookingId);
    }
}
