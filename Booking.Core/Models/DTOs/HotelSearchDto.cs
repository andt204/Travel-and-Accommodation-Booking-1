namespace BookingHotel.Core.Models.DTOs {
    public class HotelSearchDto {
        public string Keyword { get; set; } 
        public int MinCapacity { get; set; } 
        public int MaxCapacity { get; set; }
    }
}
