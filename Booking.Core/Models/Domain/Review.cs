namespace BookingHotel.Core.Models.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public int HotelId { get; set; }
        public int UserId { get; set; }

        public Hotel Hotel { get; set; }
    }
}
