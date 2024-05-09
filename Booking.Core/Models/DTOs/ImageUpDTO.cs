using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.DTOs
{
    public class ImageUpDTO
    {
        [NotMapped]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }

        public string FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
