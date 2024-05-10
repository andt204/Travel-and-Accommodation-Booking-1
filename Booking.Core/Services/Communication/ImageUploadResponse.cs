using BookingHotel.Core.Models.Domain;

namespace BookingHotel.Core.Services.Communication
{
    public class ImageUploadResponse
    {
        public Image image { get; set; }
        public string v { get; set; }

    }
}