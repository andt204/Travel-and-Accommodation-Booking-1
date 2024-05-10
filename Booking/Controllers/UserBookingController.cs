using AutoMapper;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingHotel.Controllers
{
    [Route("api/user/bookings")]
    [ApiController]
    [Authorize(Roles = Roles.User)]
    public class UserBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public UserBookingController(IBookingService bookingService, IMapper mapper)
        {
            _mapper = mapper;
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBookings([FromQuery] int pageSize = 5, [FromQuery] int pageNumber = 1)
        {
            //take user id from token
            var userId = User.FindFirst("Id")?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var bookings = await _bookingService.GetAllAsync(pageSize, pageNumber, userId);

            if (bookings == null || !bookings.Any())
                return NotFound();

            var bookingDTOs = _mapper.Map<IEnumerable<BookingHotel.Core.Models.Domain.Booking>, IEnumerable<BookingDTO>>(bookings);
            return Ok(bookingDTOs);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO booking)
        {
            var userId = User.FindFirst("Id")?.Value;
            var result = await _bookingService.CreateBookingAsync(booking, userId);
            if (result == null)
                return BadRequest();

            var bookingModel = _mapper.Map<BookingHotel.Core.Models.Domain.Booking, BookingDTO>(result);
            return Ok(bookingModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = _bookingService.RemoveAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            //take user id from token
            var userId = User.FindFirst("Id")?.Value;
            var booking = await _bookingService.GetByIdAsync(id, userId);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetBookingInvoice(int id)
        {
            //take user id from token
            var userId = User.FindFirst("Id")?.Value;
            var invoice = await _bookingService.GetInvoiceByBookingId(id, userId);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }
    }
}
