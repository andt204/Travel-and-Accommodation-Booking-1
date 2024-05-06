namespace BookingHotel.Core.Models.Domain
{
    public class Amenity
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public IList<HotelAmentities> HotelAmentities { get; set; }

    }
}
