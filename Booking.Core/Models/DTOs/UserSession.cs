using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.DTOs
{
    public record UserSession
        (
            string? Id,
            string? Email,
            string? Name,
            string? Role
        );
}
