using BookingHotel.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);
        Task<bool> IsInRoleAsync(int userId, string role);
        Task AddToRoleAsync(int userId, string role);
        Task RemoveFromRoleAsync(int userId, string role);
        Task<User> GetCurrentUser();
    }
}
