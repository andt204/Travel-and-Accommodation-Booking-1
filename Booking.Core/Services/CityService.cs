using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.IUnitOfWorks;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Repositories;
using BookingHotel.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services {
    public class CityService : ICityService {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CityService(ICityRepository cityRepository, IUnitOfWork unitOfWork) {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<City>> ListAsync(int page, int pageSize) {
            return await _cityRepository.ListAsync(page, pageSize);
        }
        public async Task<CityResponse> SaveAsync(City city) {
            try {
                if (city == null) {
                    throw new ArgumentNullException(nameof(city));
                }
                var cityEntity = new City {
                    Name = city.Name,
                    VisitedCount = city.VisitedCount,
                    Thumbnail = city.Thumbnail,
                };
                _ = _cityRepository.AddAsync(cityEntity);
                await _unitOfWork.CompleteAsync();
                return new CityResponse(cityEntity, "Create Success");
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
            existingCity.Thumbnail = city.Thumbnail;


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

    }
}
