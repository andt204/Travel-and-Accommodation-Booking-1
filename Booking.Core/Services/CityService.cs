using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using BookingHotel.Core.Repositories;
using BookingHotel.Core.Services.Communication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IThumbnailStorageService _thumbnailStorageService;

        public CityService(ICityRepository cityRepository, IUnitOfWork unitOfWork, IHostingEnvironment webHost, IHttpContextAccessor httpContextAccessor = null, IThumbnailStorageService thumbnailStorageService = null)
        {
            _cityRepository = cityRepository;
            _webHostEnvironment = webHost;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _thumbnailStorageService = thumbnailStorageService;
        }
        public async Task<IEnumerable<City>> ListAsync(int page, int pageSize) {
            return await _cityRepository.ListAsync(page, pageSize);
        }
        public async Task<CityResponse> SaveAsync(City city, IFormFile thumbnailFile) {
            try {
                if (city == null) {
                    throw new ArgumentNullException(nameof(city));
                }

                // Check if thumbnail file is provided
                if (thumbnailFile == null || thumbnailFile.Length == 0)
                {
                    return new CityResponse("Thumbnail is required.");
                }

                // Process the thumbnail file
                byte[] thumbnailBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await thumbnailFile.CopyToAsync(memoryStream);
                    thumbnailBytes = memoryStream.ToArray();
                }

                string thumbnailUrl = await _thumbnailStorageService.UploadThumbnail(thumbnailBytes);

                // Get base URL using HttpContextAccessor
                string baseUrl = $"{_httpContextAccessor.HttpContext.Request.PathBase}";
                //{ _httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}
                // Combine base URL with the path to the uploaded thumbnail
                thumbnailUrl = $"{thumbnailUrl}";
                //{ baseUrl}/
                // Set the thumbnail URL to the hotel entity
                city.ThumbnailPath = thumbnailUrl;
                
                _ = _cityRepository.AddAsync(city);
                await _unitOfWork.CompleteAsync();
                return new CityResponse(city, "Create Success");
            } catch (Exception ex) {
                return new CityResponse($"An error occurred when saving the city: {ex.Message}");
            }
        }

        public async Task<CityResponse> UpdateAsync(int id, City city) {
            var existingCity = await _cityRepository.FindByIdAsync(id);

            if (existingCity == null)
                return new CityResponse("City not found.");

            existingCity.Name = city.Name;
            existingCity.VisitedCount = city.VisitedCount;
            existingCity.ThumbnailPath = city.ThumbnailPath;


            try {
                _cityRepository.Update(existingCity);
                await _unitOfWork.CompleteAsync();

                return new CityResponse(existingCity, "Update Success");
            } catch (Exception ex) {
                // Do some logging stuff
                return new CityResponse($"An error occurred when updating the city: {ex.Message}");
            }
        }

        public async Task<CityResponse> DeleteAsync(int id) {
            var existingCity = await _cityRepository.FindByIdAsync(id);

            if (existingCity == null)
                return new CityResponse("City not found.");

            try {
                _cityRepository.Remove(existingCity);
                await _unitOfWork.CompleteAsync();

                return new CityResponse(existingCity, "Delete Success");
            } catch (Exception ex) {
                // Do some logging stuff
                return new CityResponse($"An error occurred when deleting the city: {ex.Message}");
            }
        }

        public async Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int count, int page = 1, int pageSize = 5) {
            return await _cityRepository.GetTopVisitedCitiesAsync(count, page, pageSize);
        }

        public async Task<ImageUploadResponse> UploadImage(Image image)
        {
            try
            {
                // Ensure image file name and extension are not null
                if (string.IsNullOrEmpty(image.FileName) || string.IsNullOrEmpty(image.FileExtension))
                    throw new ArgumentNullException("FileName or FileExtension", "Image file name or extension is null or empty.");

                // Combine web root path with images folder and file name
                var localFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", $"{image.FileName}{image.FileExtension}");

                // Upload image to local host
                using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    await image.File.CopyToAsync(fileStream);

                }

                // Create URL for the image
                var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                    $"{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}" +
                    $"/images/{image.FileName}{image.FileExtension}";

                // Update file path property
                image.FilePath = urlFilePath;

                // Return response indicating success
                return new ImageUploadResponse
                {
                    image = image,
                    v = "Success"
                };
            }
            catch (Exception ex)
            {
                // Return response indicating failure with error message
                return new ImageUploadResponse
                {
                    v = $"An error occurred when uploading the image: {ex.Message}"
                };
            }
        }

    }
}
