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
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }
            var identityUser = new User
            {
                Email = register.Email,
                UserName = register.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, register.Password);

            if (identityResult.Succeeded)
            {
                //generate token for verify email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                //encoder
                var code = System.Web.HttpUtility.UrlEncode(token);
                //generate link for verify email
                var link = $"https://localhost:5001/api/auth/verify-email?email={register.Email}&token={code}";
                //generate email content
                var content = $"<a href='{link}'>Click here to verify email</a>";
                //send email
                _ = SendEmail(register.Email, "Verify email", content);
                //add role for user
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    if (register.Roles != null && register.Roles.Any())
                    {
                        identityResult = await _userManager.AddToRolesAsync(identityUser, register.Roles);
                        if (identityResult.Succeeded)
                        {
                            return new RegisterResponse(true, register.Email, code, "User created success");

                        }
                    }
                }
            }
            return new RegisterResponse (false, register.Email, null!, "User created fail");
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
            {
                return new LoginResponse(true, null!, "Login Fail", null);
            }

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    //get role for this user
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //generate token
                        var token = await _tokenRepository.GenerateToken(user, roles.ToList());

                        var userToken = new LoginResponseDTO
                        {
                            Token = token
                        };

                        return new LoginResponse(true, token!, "Login success", roles.ToArray());
                        //return Object.ReferenceEquals(userToken, null) ? "Login success" : "Login failed";
                    }
                    return new LoginResponse(true, null!, "Login Fail", null);
                }
            }
            return new LoginResponse(true, null!, "Login Fail", null);
        }

        public Task<GeneralResponse> ConfirmVerifyEmail(string email, string token)
        {
            if(email == null || token == null)
            {
                return Task.FromResult(new GeneralResponse(false, "Email or token is null"));
            }
            var user = _userManager.FindByEmailAsync(email).Result;
            if(user == null)
            {
                return Task.FromResult(new GeneralResponse(false, "User not found"));
            }

            //decode token
            token = System.Web.HttpUtility.UrlDecode(token);
            //confirm verify email
            var result = _userManager.ConfirmEmailAsync(user, token).Result;
            //if success
            if(result.Succeeded)
            {
                return Task.FromResult(new GeneralResponse(true, "Verify email success"));
            }
            return Task.FromResult(new GeneralResponse(false, "Verify email fail"));
        }
    }
}
