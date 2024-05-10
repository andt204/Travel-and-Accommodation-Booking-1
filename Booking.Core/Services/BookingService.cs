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
        private readonly IRoomRepository _roomRepository;
        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IIdentityService identityService, IRoomRepository roomRepository)
        {
            _unitOfWorks = unitOfWork;
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Booking> CreateBookingAsync(BookingDTO booking, string userId)
        {
            if(booking == null)
            {
                throw new ArgumentNullException(nameof(booking));
            }
            var roomCheckStatus = await _roomRepository.CheckRoomStatus(booking.RoomId);

            if(roomCheckStatus == true)
            {
                throw new Exception("Room is not available");
            }
            
            var bookingEntity = new Booking
            {
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                status = true,
                RoomId = booking.RoomId,
                UserId = userId
            };
            _ = _bookingRepository.CreateBookingAsync(bookingEntity);
            await _unitOfWorks.CompleteAsync();
            return bookingEntity;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(int pageSize, int pageNumber,string user)
        {
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return await _bookingRepository.GetAllAsync(pageSize, pageNumber, user);
        }

        public async Task<Booking> GetByIdAsync(int id, string userId)
        {
            if (id == null)
            {
                 throw new Exception("Id is null");
            }
            return await _bookingRepository.GetByIdAsync(id, userId);
        }

        public async Task<Invoice> GetInvoiceByBookingId(int bookingId, string userId)
        {
            return await _bookingRepository.GetInvoiceByBookingId(bookingId, userId);
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
