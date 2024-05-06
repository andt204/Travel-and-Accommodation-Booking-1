namespace BookingHotel.Core.Models.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public bool Status { get; set; }

        public int HotelId { get; set; }
        public int RoomClassId { get; set; }

        public Hotel Hotel { get; set; }
        public RoomClasses RoomClasses { get; set; }

    }
}
