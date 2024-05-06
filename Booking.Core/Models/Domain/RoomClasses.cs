namespace BookingHotel.Core.Models.Domain
{
    public class RoomClasses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPerson { get; set; }
        public decimal Price { get; set; }
        public int DiscountId { get; set; }
    }
}
