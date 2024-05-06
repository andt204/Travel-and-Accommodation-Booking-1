using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool status { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}
