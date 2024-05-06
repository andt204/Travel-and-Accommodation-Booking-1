using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services.Communication {
    public class CityResponse : BaseResponse {
        public City City { get; private set; }

        private CityResponse(bool success, string message, City city) : base(success, message) {
            City = city;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public CityResponse(City city, string message) : this(true, message, city) { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public CityResponse(string message) : this(false, message, null) { }
    }
}
