using AutoMapper;
using BookingHotel.Core.Extensions;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using BookingHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers {
    [Route("/api/hotels")]
    [Authorize]
    public class HotelsController : Controller {

        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;

        public HotelsController(IHotelService hotelService, IMapper mapper) {
            _hotelService = hotelService;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery]  int pageSize = 10) {
            var hotels = await _hotelService.ListAsync(page, pageSize);
            var hotelDtos = _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelDto>>(hotels);
            return Ok(hotelDtos);
        }


        [HttpPost]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> PostAsync([FromBody] SaveHotelDto resource) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var hotel = _mapper.Map<SaveHotelDto, Hotel>(resource);
            var result = await _hotelService.SaveAsync(hotel);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveHotelDto resource) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var hotel = _mapper.Map<SaveHotelDto, Hotel>(resource);
            var result = await _hotelService.UpdateAsync(id, hotel);

            if (!result.Success)
                return BadRequest(result.Message);

            var hotelDto = _mapper.Map<Hotel, HotelDto>(result.Hotel);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> DeleteAsync(int id) {
            var result = await _hotelService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var hotelDto = _mapper.Map<Hotel, HotelDto>(result.Hotel);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> GetByIdAsync(int id) {
            var result = await _hotelService.FindByIdAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            var hotelDto = _mapper.Map<Hotel, HotelDto>(result.Hotel);
            return Ok(result);
        }
        [HttpGet("search")]
        [Authorize(Roles = Roles.User + ", " + Roles.Owner)]
        public async Task<IActionResult> SearchAsync([FromQuery] HotelSearchDto hotelSearch, int page = 1, int pageSize = 3) {
            if (hotelSearch == null)
                return BadRequest("Search criteria cannot be empty.");

            var hotels = await _hotelService.SearchAsync(hotelSearch.Keyword, hotelSearch.MinCapacity, hotelSearch.MaxCapacity, page, pageSize);

            var hotelDtos = _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelDto>>(hotels);

            return Ok(hotelDtos);
        }
    }
}

