namespace BookingHotel.Core.Models.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool status { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public Room Room { get; set; }
    }
}
