using BookingHotel.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingHotel.Core.Services.Communication.ServiceResponse;

namespace BookingHotel.Core.IServices
{
    public interface IAuthService
    {
        Task<LoginResponse> Signin(LoginDTO loginDTO);
        Task<RegisterResponse> Register(RegisterDTO register);
        Task<GeneralResponse> ConfirmVerifyEmail(string email, string token);
    }
}
