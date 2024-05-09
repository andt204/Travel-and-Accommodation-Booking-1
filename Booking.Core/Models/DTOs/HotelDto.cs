using Microsoft.AspNetCore.Http;

namespace BookingHotel.Core.Models.DTOs {
    public class HotelDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int Rating { get; set; }
        public int NumOfRoom { get; set; }
        public int GalleryId { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
