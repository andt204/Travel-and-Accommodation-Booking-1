using AutoMapper;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers
{
    [Route("api/user/bookings")]
    [ApiController]
    public class UserBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper  _mapper;
        public UserBookingController(IBookingService bookingService, IMapper mapper)
        {
            _mapper = mapper;
            _bookingService = bookingService;
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            var result = await _bookingService.GetAllAsync();

            if(result == null)
            {
                return NotFound();
            }
            var resouces = _mapper.Map<IEnumerable<BookingHotel.Core.Models.Domain.Booking>, IEnumerable<BookingDTO>>(result);
            return Ok(resouces);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO booking)
        {
            // Xử lý yêu cầu tạo mới đặt vé của người dùng
            var result = _bookingService.CreateBooking(booking);
            //mapping
            var bookingModel = _mapper.Map<BookingDTO>(booking);
            if(result == null)
            {
                return BadRequest();
            }

            return Ok(bookingModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = _bookingService.RemoveAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            // Xử lý yêu cầu lấy thông tin đặt vé của người dùng theo ID
            var result = await _bookingService.GetByIdAsync(id);
            if (result == null)
            {
                //return string not found
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/invoice")]
        public async Task<IActionResult> GetBookingInvoice(int id)
        {
            // Xử lý yêu cầu lấy hóa đơn của đặt vé của người dùng theo ID
            var result = await _bookingService.GetInvoiceByBookingId(id);
            if (result == null)
            {
                //return string not found
                return NotFound();
            }
            return Ok(result);
        }
    }
}
