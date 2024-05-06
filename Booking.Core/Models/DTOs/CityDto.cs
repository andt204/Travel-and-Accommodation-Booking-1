﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.DTOs {
    public class CityDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VisitedCount { get; set; }
        public string Thumbnail { get; set; }
    }
}
