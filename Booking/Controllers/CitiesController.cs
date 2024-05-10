using AutoMapper;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using BookingHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace BookingHotel.Controllers {
    [Route("/api/cities")]
    [Authorize]
    public class CitiesController : Controller {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly IThumbnailStorageService _thumbnailStorageService;

        public CitiesController(ICityService cityService, IMapper mapper, IThumbnailStorageService thumbnailStorageService = null)
        {
            _cityService = cityService;
            _mapper = mapper;
            _thumbnailStorageService = thumbnailStorageService;
        }
        [Authorize(Roles = Roles.User)]
        [HttpGet]
        public async Task<IEnumerable<CityDto>> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
            var cities = await _cityService.ListAsync(page, pageSize);

            var resources = _mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(cities);
            return resources;
        }

        [Authorize(Roles = Roles.Owner)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] SaveCityDto resource) {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState.GetErrorMessages());

            //generate file image url to save db
            string thumbnailUrl = await _thumbnailStorageService.GenerateUrlOfImage(resource);

            var city = _mapper.Map<SaveCityDto, City>(resource);

            city.ThumbnailPath = thumbnailUrl;

            var result = await _cityService.SaveAsync(city, resource.ThumbnailFile);

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

        private void ValidateFileUpload(ImageUpDTO imageUpDTO)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(imageUpDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "Upsupported file extension");
            }

            if (imageUpDTO.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Files", "File size cannot exceed 2MB");
            }

            /*if (ModelState.ErrorCount > 0)
            {
                throw new ValidationException(ModelState);
            }*/
        }

        [HttpPut("{id}/thumbnail")]
        public Task<IActionResult> GetThumbnailAsync([FromForm] ImageUpDTO image, int id)
        {
            ValidateFileUpload(image);


            var imageModel = _mapper.Map<ImageUpDTO, Image>(image);

            var upImg = _cityService.UploadImage(imageModel);

            var result = _cityService.UpdateAsync(id, new City { ThumbnailPath = upImg.Result.ToString()});

            if (!result.Result.Success)
                return Task.FromResult<IActionResult>(BadRequest(result.Result.Message));

            return Task.FromResult<IActionResult>(Ok(result.Result));
        }
    }
}
