using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.Domain {
    public class Hotel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public int CityId { get; set; }

        public int Rating { get; set; }
        public int NumOfRoom { get; set; }
        public int GalleryId { get; set; }

        public Gallery Gallery { get; set; }
        public City City { get; set; }
        public User User { get; set; }
        public IList<HotelAmentities> HotelAmentities { get; set; }
    }
}