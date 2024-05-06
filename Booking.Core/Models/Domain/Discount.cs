namespace BookingHotel.Core.Models.Domain
{
    public class Discount
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Percent { get; set; }
        public string Description { get; set; }
    }
}
