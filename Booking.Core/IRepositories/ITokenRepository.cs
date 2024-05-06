using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IRepositories
{
    public interface ITokenRepository
    {
        Task<string> GenerateToken(IdentityUser user, List<string> roles);
    }
}
