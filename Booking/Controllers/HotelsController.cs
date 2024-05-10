using AutoMapper;
using BookingHotel.Core.Extensions;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using BookingHotel.Core.Services;
using BookingHotel.Core.Services.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers {
    [Route("/api/hotels")]
    //[Authorize]
    public class HotelsController : Controller {

        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        private readonly IThumbnailStorageService _thumbnailStorageService;

        public HotelsController(IHotelService hotelService, IMapper mapper, IThumbnailStorageService thumbnailStorageService) {
            _hotelService = hotelService;
            _thumbnailStorageService = thumbnailStorageService;
            _mapper = mapper;
        }
        [HttpGet]
        //[Authorize(Roles = Roles.User + ", " + Roles.Owner)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery]  int pageSize = 10) {
            var hotels = await _hotelService.ListAsync(page, pageSize);
            var hotelDtos = _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelDto>>(hotels);
            return Ok(hotelDtos);
        }

        [HttpPost]
        //[Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> PostAsync([FromForm] SaveHotelDto resource) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (resource.ThumbnailFile == null || resource.ThumbnailFile.Length == 0) {
                return BadRequest("Thumbnail is required.");
            }
            //generate file image url to save db
            string thumbnailUrl = await _thumbnailStorageService.GenerateUrlOfImage(resource);

            // Map SaveHotelDto to Hotel entity
            var hotel = _mapper.Map<SaveHotelDto, Hotel>(resource);

            // Set the thumbnail property of the hotel entity
            hotel.ThumbnailPath = thumbnailUrl;

            var result = await _hotelService.SaveAsync(hotel, resource.ThumbnailFile);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }




        [HttpPut("{id}")]
        //[Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> PutAsync(int id, [FromForm] SaveHotelDto resource) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (resource.ThumbnailFile == null || resource.ThumbnailFile.Length == 0) {
                return BadRequest("Thumbnail is required.");
            }
            // Process the thumbnail file
            string thumbnailUrl;
            using (var memoryStream = new MemoryStream()) {
                await resource.ThumbnailFile.CopyToAsync(memoryStream);

                // Convert byte array to Base64 string
                string base64String = Convert.ToBase64String(memoryStream.ToArray());

                // Generate the thumbnail URL (or path)
                thumbnailUrl = $"data:image/jpeg;base64,{base64String}";
            }
            var hotel = _mapper.Map<SaveHotelDto, Hotel>(resource);

            // Set the thumbnail property of the hotel entity
            hotel.ThumbnailPath = thumbnailUrl;
            var result = await _hotelService.UpdateAsync(id, hotel, resource.ThumbnailFile);

            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = Roles.Owner)]
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

