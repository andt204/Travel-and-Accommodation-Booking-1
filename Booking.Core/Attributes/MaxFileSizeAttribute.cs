using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Attributes {
    public class MaxFileSizeAttribute : ValidationAttribute {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize) {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is IFormFile file) {
                if (file.Length > _maxFileSize) {
                    return new ValidationResult($"The file size cannot exceed {_maxFileSize} bytes.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
