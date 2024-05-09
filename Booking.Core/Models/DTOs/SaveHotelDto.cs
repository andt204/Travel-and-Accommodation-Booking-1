using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookingHotel.Core.Models.DTOs {
    public class SaveHotelDto {
        [Required(ErrorMessage = "Hotel name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City ID is required")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Number of rooms is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of rooms must be at least 1")]
        public int NumOfRoom { get; set; }

        [Required(ErrorMessage = "Gallery ID is required")]
        public int GalleryId { get; set; }

        [Required(ErrorMessage = "Thumbnail File is required")]
        public IFormFile ThumbnailFile { get; set; }

    }
}

