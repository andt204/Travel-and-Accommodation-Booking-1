using BookingHotel.Core.Context;
using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IUnitOfWorks {
    public class UnitOfWork : IUnitOfWork {
        private readonly BookingHotelDbContext _context;
        private IBookingRepository _bookingRepository;
        private IHotelRepository _hotelRepository;
        public UnitOfWork(BookingHotelDbContext context) {
            _context = context;
        }
        public async Task CompleteAsync() {
            await _context.SaveChangesAsync();
        }
        public IBookingRepository BookingRepository => _bookingRepository = _bookingRepository ?? new BookingRepository(_context);
        public IHotelRepository HotelRepository => _hotelRepository = _hotelRepository ?? new HotelRepository(_context);

    }
}
