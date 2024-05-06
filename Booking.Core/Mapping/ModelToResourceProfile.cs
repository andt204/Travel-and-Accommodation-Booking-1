
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Models.Domain;
using System;


namespace BookingHotel.Core.Mapping {
    public class ModelToResourceProfile : AutoMapper.Profile {
        public ModelToResourceProfile() {
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, SaveHotelDto>().ReverseMap();
        }
    }
}

