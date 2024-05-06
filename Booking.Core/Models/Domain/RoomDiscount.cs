using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.Domain
{
    public class RoomDiscount
    {
        public int ID { get; set; }
        public int DiscountId { get; set; }
        public int RoomClassesId { get; set; }

        public RoomClasses RoomClasses { get; set; }
        public Discount Discount { get; set; }
    }
}
