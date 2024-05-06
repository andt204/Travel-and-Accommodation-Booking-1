using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services.Communication;
using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWorks = unitOfWork;
            _bookingRepository = bookingRepository;
        }

        public async Task CreateBooking(BookingDTO booking)
        {
            if(booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }
            _ = _bookingRepository.CreateBooking(booking);
            await _unitOfWorks.CompleteAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            if (id == null)
            {
                 throw new Exception("Id is null");
            }
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Invoice> GetInvoiceByBookingId(int bookingId)
        {
            return await _bookingRepository.GetInvoiceByBookingId(bookingId);
        }

        public async Task RemoveAsync(int Id)
        {
            if(Id == null)
            {
                throw new Exception("Id is null");
            }
            _ = _bookingRepository.RemoveAsync(Id);
            await _unitOfWorks.CompleteAsync();

        }

        public async Task UpdateAsync(Booking booking)
        {
            if(booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }
            await _bookingRepository.UpdateAsync(booking);
            _ = _unitOfWorks.CompleteAsync();
        }
    }
}
