using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Repositories
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(BookingHotelDbContext context) : base(context)
        {
        }

        public async Task CreateBooking(BookingDTO booking)
        {
            var bookingEntity = new Booking
            {
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                status = booking.status,
                RoomId = booking.RoomId,
                UserId = booking.UserId
            };
            await _context.Bookings.AddAsync(bookingEntity);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Invoice> GetInvoiceByBookingId(int bookingId)
        {
            return await _context.Invoices.FirstOrDefaultAsync(x => x.BookingId == bookingId);
        }

        public async Task RemoveAsync(int Id)
        {
            var bookingFind = await _context.Bookings.FindAsync(Id);
            if (bookingFind == null)
            {
                throw new Exception("Booking not found");
            }
            _context.Bookings.Remove(bookingFind);
        }

        public async Task UpdateAsync(Booking booking)
        {
            await Task.Run(() =>
            {
                _context.Bookings.Update(booking);
            });
        }
    }
}
