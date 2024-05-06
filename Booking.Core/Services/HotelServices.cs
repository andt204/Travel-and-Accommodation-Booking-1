using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Repositories;
using BookingHotel.Core.Services.Communication;
using BookingHotel.Core.Models.Domain;

namespace BookingHotel.Core.Services {
    public class HotelServices : IHotelServices {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HotelServices(IHotelRepository hotelRepository, IUnitOfWork unitOfWork) {
            _hotelRepository = hotelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Hotel>> ListAsync() {
            return await _hotelRepository.ListAsync();
        }
        public async Task<HotelResponse> SaveAsync(Hotel hotel) {
            try {
                if (hotel == null) {
                    throw new ArgumentNullException(nameof(hotel));
                }
                var hotelEntity = new Hotel {
                    Name = hotel.Name,
                    Description = hotel.Description,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    Rating = hotel.Rating,
                    NumOfRoom = hotel.NumOfRoom,
                    GalleryId = hotel.GalleryId
                };
                _ = _hotelRepository.AddAsync(hotelEntity);
                await _unitOfWork.CompleteAsync();
                return new HotelResponse(hotelEntity);
            } catch (Exception ex) {
                return new HotelResponse($"An error occurred when saving the hotel: {ex.Message}");
            }
        }
        //public async Task<HotelResponse> SaveAsync(SaveHotelDto hotel) {
        //    try {
        //        if (hotel == null) {
        //            throw new ArgumentNullException(nameof(hotel));
        //        }
        //        _ = _hotelRepository.AddAsync(hotel);
        //        await _unitOfWork.CompleteAsync();
        //        return new HotelResponse(hotel);
        //    } catch (Exception ex) {
        //        return new HotelResponse($"An error occurred when saving the hotel: {ex.Message}");
        //    }
        //}

        public async Task<HotelResponse> UpdateAsync(int id, Hotel hotel) {
            var existingHotel = await _hotelRepository.FindByIdAsync(id);

            if (existingHotel == null)
                return new HotelResponse("Category not found.");

            existingHotel.Name = hotel.Name;
            existingHotel.Description = hotel.Description;
            existingHotel.Address = hotel.Address;
            existingHotel.CityId = hotel.CityId;
            existingHotel.Rating = hotel.Rating;
            existingHotel.NumOfRoom = hotel.NumOfRoom;
            existingHotel.GalleryId = hotel.GalleryId;

            try {
                _hotelRepository.Update(existingHotel);
                await _unitOfWork.CompleteAsync();

                return new HotelResponse(existingHotel);
            } catch (Exception ex) {
                // Do some logging stuff
                return new HotelResponse($"An error occurred when updating the category: {ex.Message}");
            }
        }

        //public async Task<HotelResponse> DeleteAsync(int id) {
        //    var existingHotel = await _hotelRepository.FindByIdAsync(id);

        //    if (existingHotel == null)
        //        return new HotelResponse("Category not found.");

        //    try {
        //        _hotelRepository.Remove(existingHotel);
        //        await _unitOfWork.CompleteAsync();

        //        return new HotelResponse(existingHotel);
        //    } catch (Exception ex) {
        //        // Do some logging stuff
        //        return new HotelResponse($"An error occurred when deleting the category: {ex.Message}");
        //    }
        //}
    }
}
