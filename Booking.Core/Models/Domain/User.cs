using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models.Domain
{
    public class User : IdentityUser
    {
        public User() { }
        public int Age { get; set; }
    }

}
