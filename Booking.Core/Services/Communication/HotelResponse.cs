using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services.Communication {
    public class HotelResponse : BaseResponse {
        public Hotel Hotel { get; private set; }

        private HotelResponse(bool success, string message, Hotel hotel) : base(success, message) {
            Hotel = hotel;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public HotelResponse(Hotel hotel) : this(true, "Item added successfully!", hotel) { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public HotelResponse(string message) : this(false, message, null) { }
    }
}
