using AutoMapper;
using BookingHotel.Core.Extensions;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers {
    [Route("/api/[controller]")]
    public class HotelsController : Controller {

        private readonly IHotelServices _hotelService;
        private readonly IMapper _mapper;

        public HotelsController(IHotelServices hotelService, IMapper mapper) {
            _hotelService = hotelService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<HotelDto>> GetAllAsync() {
            var hotels = await _hotelService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelDto>>(hotels);
            return resources;
        }

        [HttpPost]
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
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveHotelDto resource) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var hotel = _mapper.Map<SaveHotelDto, Hotel>(resource);
            var result = await _hotelService.UpdateAsync(id, hotel);

            if (!result.Success)
                return BadRequest(result.Message);

            var hotelDto = _mapper.Map<Hotel, HotelDto>(result.Hotel);
            return Ok(hotelDto);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAsync(int id) {
        //    var result = await _hotelService.DeleteAsync(id);

        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    var hotelDto = _mapper.Map<Hotel, HotelDto>(result.Hotel);
        //    return Ok(hotelDto);
        //}
        //public async Task<IActionResult> CancelBooking(int id) {
        //    var result = _hotelService.RemoveAsync(id);
        //    if (result == null) {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}
    }
}

