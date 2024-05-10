using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.Domain {
    public class City {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VisitedCount { get; set; }
        public string ThumbnailPath { get; set; }
        [NotMapped]
        public IFormFile ThumbnailFile { get; set; }
    }
}
