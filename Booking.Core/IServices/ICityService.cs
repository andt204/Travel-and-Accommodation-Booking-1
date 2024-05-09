using Azure;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services.Communication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices {
    public interface ICityService {
        Task<IEnumerable<City>> ListAsync(int page, int pageSize);
        Task<CityResponse> SaveAsync(City city);
        Task<CityResponse> UpdateAsync(int id, City city);
        Task<CityResponse> DeleteAsync(int id);
        Task<IEnumerable<City>> GetTopVisitedCitiesAsync(int count, int page = 1, int pageSize = 5);

        Task<ImageUploadResponse> UploadImage(Image image);
    }
}
