using AutoMapper;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using BookingHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace BookingHotel.Controllers {
    [Route("/api/cities")]
    [Authorize]
    public class CitiesController : Controller {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;

        public CitiesController(ICityService cityService, IMapper mapper) {
            _cityService = cityService;
            _mapper = mapper;
        }
        [Authorize(Roles = Roles.User + ", " + Roles.Owner)]
        [HttpGet]
        public async Task<IEnumerable<CityDto>> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
            var cities = await _cityService.ListAsync(page, pageSize);
            var resources = _mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(cities);
            return resources;
        }
        [Authorize(Roles = Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCityDto resource) {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState.GetErrorMessages());

            var hotel = _mapper.Map<SaveCityDto, City>(resource);
            var result = await _cityService.SaveAsync(hotel);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCityDto resource) {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState.GetErrorMessages());

            var city = _mapper.Map<SaveCityDto, City>(resource);
            var result = await _cityService.UpdateAsync(id, city);

            if (!result.Success)
                return BadRequest(result.Message);

            var cityDto = _mapper.Map<City, CityDto>(result.City);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<IActionResult> DeleteAsync(int id) {
            var result = await _cityService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var cityDto = _mapper.Map<City, CityDto>(result.City);
            return Ok(result);
        }

        [HttpGet("trending")]
        public async Task<ActionResult<IEnumerable<CityDto>>> TrendingAsync([FromQuery] int count, [FromQuery] int page = 1, [FromQuery] int pageSize = 5) {
            var topVisitedCities = await _cityService.GetTopVisitedCitiesAsync(count, page, pageSize);
            var cityDtos = _mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(topVisitedCities);
            return Ok(cityDtos);
        }
    }
}
