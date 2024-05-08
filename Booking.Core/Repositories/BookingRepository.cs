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
using System.Security.Claims;

namespace BookingHotel.Core.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookingHotelDbContext context) : base(context)
        {
        }

        public async Task CreateBookingAsync(Booking booking)
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

        public async Task<IEnumerable<Booking>> GetAllAsync(int pageSize, int pageNumber, string user)
        {
            if (user == null)
            {
                return null;
            }

            var bookings = _context.Bookings
                .Where(x => x.UserId == user)
                .OrderBy(c => c.StartDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await bookings.ToListAsync();
        }


        public async Task<Booking> GetByIdAsync(int id, string userId)
        {
            if (id == null)
            {
                throw new Exception("Id is null");
            }
            return await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
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
