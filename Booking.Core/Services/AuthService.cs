using BookingHotel.Core.IRepositories;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingHotel.Core.Services.Communication.ServiceResponse;

namespace BookingHotel.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly ITokenRepository _tokenRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<User> userManager, ITokenRepository tokenRepository, RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _roleManager = roleManager;
        }

        public async Task<RegisterResponse> Register(RegisterDTO register)
        {
            //check null
            if (register == null)
                throw new ArgumentNullException(nameof(register));
            
            var identityUser = new User
            {
                Email = register.Email,
                UserName = register.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, register.Password);

            if (!identityResult.Succeeded)
                return new RegisterResponse(false, register.Email, null!, "User creation failed");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var code = System.Web.HttpUtility.UrlEncode(token);
            var link = $"https://localhost:5001/api/auth/verify-email?email={register.Email}&token={code}";
            var content = $"<a href='{link}'>Click here to verify email</a>";
            _ = await SendEmail(register.Email, "Verify email", content);

            if (!_userManager.Options.SignIn.RequireConfirmedEmail || (register.Roles == null || !register.Roles.Any()))
                return new RegisterResponse(true, register.Email, code, "User created successfully");

            identityResult = await _userManager.AddToRolesAsync(identityUser, register.Roles);
            if (!identityResult.Succeeded)
                return new RegisterResponse(false, register.Email, null!, "Failed to add roles to user");

            return new RegisterResponse(true, register.Email, code, "User created successfully");
        }
        //function to send email
        public async Task<string> SendEmail(string email, string subject, string htmlMessage)
        {
            await _sendEmailService.SendEmailAsync(email, subject, htmlMessage);
            return "Send email success";
        }

        public async Task<LoginResponse> Signin(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(true, null!, "Login Fail", null);

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !user.EmailConfirmed)
                return new LoginResponse(true, null!, user == null ? "User not found" : "Email not verified", null);

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
                return new LoginResponse(true, null!, "Login Fail", null);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
                return new LoginResponse(true, null!, "User has no roles assigned", null);

            var token = _tokenRepository.GenerateToken(user, roles.ToList());
            return new LoginResponse(true, token!, "Login success", roles.ToArray());
        }


        public async Task<GeneralResponse> ConfirmVerifyEmail(string email, string token)
        {
            if (email == null || token == null)
                return new GeneralResponse(false, "Email or token is null");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new GeneralResponse(false, "User not found");

            token = System.Web.HttpUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded
                ? new GeneralResponse(true, "Verify email success")
                : new GeneralResponse(false, "Verify email failed");
        }
    }
}
