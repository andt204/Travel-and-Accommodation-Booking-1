using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.UserRoles;
using BookingHotel.Core.Repositories;
using BookingHotel.Core.Services.Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace BookingHotel.Core.Services
{
    public class HotelService : IHotelService {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IThumbnailStorageService _thumbnailStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HotelService(IHotelRepository hotelRepository, IUnitOfWork unitOfWork, IThumbnailStorageService thumbnailStorageService, IHttpContextAccessor httpContextAccessor) {
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
            _thumbnailStorageService = thumbnailStorageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Hotel>> ListAsync(int page, int pageSize) {
            return await _hotelRepository.ListAsync(page, pageSize);
        }

        //public async Task<HotelResponse> SaveAsync(Hotel hotel) {
        //    try {
        //        if (hotel == null) {
        //            throw new ArgumentNullException(nameof(hotel));
        //        }
        //        var hotelEntity = new Hotel {
        //            Name = hotel.Name,
        //            Description = hotel.Description,
        //            Address = hotel.Address,
        //            CityId = hotel.CityId,
        //            Rating = hotel.Rating,
        //            NumOfRoom = hotel.NumOfRoom,
        //            GalleryId = hotel.GalleryId
        //        };
        //        _ = _hotelRepository.AddAsync(hotelEntity);
        //        await _unitOfWork.CompleteAsync();
        //        return new HotelResponse(hotelEntity, "Create Success");
        //    } catch (Exception ex) {
        //        return new HotelResponse($"An error occurred when saving the hotel: {ex.Message}");
        //    }
        //}
        //public async Task<HotelResponse> SaveAsync(Hotel hotel, IFormFile thumbnailFile) {
        //    try {
        //        if (hotel == null) {
        //            throw new ArgumentNullException(nameof(hotel));
        //        }

        //        // Check if thumbnail file is provided
        //        if (thumbnailFile == null || thumbnailFile.Length == 0) {
        //            return new HotelResponse("Thumbnail is required.");
        //        }

        //        // Process the thumbnail file
        //        byte[] thumbnailBytes;
        //        using (var memoryStream = new MemoryStream()) {
        //            await thumbnailFile.CopyToAsync(memoryStream);
        //            thumbnailBytes = memoryStream.ToArray();
        //        }

        //        // Save thumbnail to storage
        //        string thumbnailUrl = await _thumbnailStorageService.UploadThumbnail(thumbnailBytes);
        //        // Set the thumbnail URL to the hotel entity
        //        hotel.ThumbnailPath = thumbnailUrl;

        //        // Save hotel entity
        //        _ = _hotelRepository.AddAsync(hotel);
        //        await _unitOfWork.CompleteAsync();

        //        return new HotelResponse(hotel, "Create Success");
        //    } catch (Exception ex) {
        //        return new HotelResponse($"An error occurred when saving the hotel: {ex.Message}");
        //    }
        //}
        public async Task<HotelResponse> SaveAsync(Hotel hotel, IFormFile thumbnailFile) {
            try {
                if (hotel == null) {
                    throw new ArgumentNullException(nameof(hotel));
                }

                // Check if thumbnail file is provided
                if (thumbnailFile == null || thumbnailFile.Length == 0) {
                    return new HotelResponse("Thumbnail is required.");
                }

                // Process the thumbnail file
                byte[] thumbnailBytes;
                using (var memoryStream = new MemoryStream()) {
                    await thumbnailFile.CopyToAsync(memoryStream);
                    thumbnailBytes = memoryStream.ToArray();
                }

                // Save thumbnail to storage
                string thumbnailUrl = await _thumbnailStorageService.UploadThumbnail(thumbnailBytes);

                // Get base URL using HttpContextAccessor
                string baseUrl = $"{_httpContextAccessor.HttpContext.Request.PathBase}";
                //{ _httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}
                // Combine base URL with the path to the uploaded thumbnail
                thumbnailUrl = $"{thumbnailUrl}";
                //{ baseUrl}/
                // Set the thumbnail URL to the hotel entity
                hotel.ThumbnailPath = thumbnailUrl;

                // Save hotel entity
                _ = _hotelRepository.AddAsync(hotel);
                await _unitOfWork.CompleteAsync();

                return new HotelResponse(hotel, "Create Success");
            } catch (Exception ex) {
                return new HotelResponse($"An error occurred when saving the hotel: {ex.Message}");
            }
        }

        public async Task<HotelResponse> UpdateAsync(int id, Hotel hotel, IFormFile thumbnailFile) {
            var existingHotel = await _hotelRepository.FindByIdAsync(id);

            if (existingHotel == null)
                return new HotelResponse("Hotel not found.");

            try {
                // Check if a new thumbnail file is provided
                if (thumbnailFile != null && thumbnailFile.Length > 0) {
                    // Process the new thumbnail file
                    byte[] thumbnailBytes;
                    using (var memoryStream = new MemoryStream()) {
                        await thumbnailFile.CopyToAsync(memoryStream);
                        thumbnailBytes = memoryStream.ToArray();
                    }

                    // Save the new thumbnail to storage
                    string thumbnailUrl = await _thumbnailStorageService.UploadThumbnail(thumbnailBytes);

                    // Update the thumbnail path in the hotel entity
                    existingHotel.ThumbnailPath = thumbnailUrl;
                }

                // Update other hotel properties if provided
                if (!string.IsNullOrEmpty(hotel.Name))
                    existingHotel.Name = hotel.Name;

                if (!string.IsNullOrEmpty(hotel.Description))
                    existingHotel.Description = hotel.Description;

                if (!string.IsNullOrEmpty(hotel.Address))
                    existingHotel.Address = hotel.Address;

                if (hotel.CityId != 0)
                    existingHotel.CityId = hotel.CityId;

                if (hotel.NumOfRoom != 0)
                    existingHotel.NumOfRoom = hotel.NumOfRoom;

                if (hotel.GalleryId != 0)
                    existingHotel.GalleryId = hotel.GalleryId;

                // Update the hotel entity in the repository
                _hotelRepository.Update(existingHotel);
                await _unitOfWork.CompleteAsync();

                return new HotelResponse(existingHotel, "Update Success");
            } catch (Exception ex) {
                // Log the error
                // Return error response
                return new HotelResponse($"An error occurred when updating the hotel: {ex.Message}");
            }
        }


        public async Task<HotelResponse> DeleteAsync(int id) {
            var existingHotel = await _hotelRepository.FindByIdAsync(id);

            if (existingHotel == null)
                return new HotelResponse("Hotel not found.");

            try {

                _hotelRepository.Remove(existingHotel);
                await _unitOfWork.CompleteAsync();

                return new HotelResponse(existingHotel, "Delete Success");
            } catch (Exception ex) {
                // Do some logging stuff
                return new HotelResponse($"An error occurred when deleting the hotel: {ex.Message}");
            }
        }

        public async Task<HotelResponse> FindByIdAsync(int id) {
            var existingHotel = await _hotelRepository.FindByIdAsync(id);

            if (existingHotel == null)
                return new HotelResponse("Hotel not found.");

            try {
                return new HotelResponse(existingHotel, "Found Success");
            } catch (Exception ex) {
                // Do some logging stuff
                return new HotelResponse($"An error occurred when finding the hotel: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Hotel>> SearchAsync(string keyword = null, int? minCapacity = null, int? maxCapacity = null, int page = 1, int pageSize = 10) {
            if (string.IsNullOrEmpty(keyword) && minCapacity == null && maxCapacity == null) {
                return await _hotelRepository.ListAsync(page, pageSize);
            }

            return await _hotelRepository.SearchAsync(keyword ?? "", minCapacity ?? 0, maxCapacity ?? 0, page, pageSize);
        }

        [HttpPut("{id}/thumbnail")]
        [Authorize(Roles = Roles.Owner)]
        public async Task<HotelResponse> UpdateThumbnailAsync(int id, Hotel hotel, [FromForm] IFormFile thumbnailFile) {
            var existingHotel = await _hotelRepository.FindByIdAsync(id);

            if (existingHotel == null)
                return new HotelResponse("Hotel not found.");

            try {
                // Check if a new thumbnail file is provided
                if (thumbnailFile != null && thumbnailFile.Length > 0) {
                    // Process the new thumbnail file
                    byte[] thumbnailBytes;
                    using (var memoryStream = new MemoryStream()) {
                        await thumbnailFile.CopyToAsync(memoryStream);
                        thumbnailBytes = memoryStream.ToArray();
                    }

                    // Save the new thumbnail to storage
                    string thumbnailUrl = await _thumbnailStorageService.UploadThumbnail(thumbnailBytes);

                    // Update the thumbnail path in the hotel entity
                    existingHotel.ThumbnailPath = thumbnailUrl;
                }

                _hotelRepository.Update(existingHotel);
                await _unitOfWork.CompleteAsync();

                return new HotelResponse(existingHotel, "Update Success");
            } catch (Exception ex) {
                // Log the error
                // Return error response
                return new HotelResponse($"An error occurred when updating the hotel: {ex.Message}");
            }
        }
    }
}
