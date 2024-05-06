using AutoMapper;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BookingHotel.Core.Mapping {
    public class ResourceToModelProfile : Profile {
        public ResourceToModelProfile() {
            CreateMap<SaveHotelDto, Hotel>();
        }
    }
}
