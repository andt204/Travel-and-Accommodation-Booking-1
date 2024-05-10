using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.DTOs {
    public class SaveCityDto {
        [Required(ErrorMessage = "City name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Thumbnail is required")]
        public IFormFile ThumbnailFile { get; set; }

    }
}
