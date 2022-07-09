using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Application.Common;
using TrustBank.Application.Dtos;
using TrustBank.Application.Responses;
using TrustBank.Application.Services.Interfaces;
using TrustBank.Core.Models;

namespace TrustBank.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<JwtData> _jwtDataOptions;
        private readonly ITokenService<JwtData> _tokenService;

        public AuthService(UserManager<Customer> userManager, SignInManager<Customer> signInManager, RoleManager<IdentityRole> roleManager, IOptions<JwtData> jwtDataOptions, ITokenService<JwtData> tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtDataOptions = jwtDataOptions;
            _tokenService = tokenService;
        }
        public async Task<Response<LoginResponseDto>> Login(LoginDto model)
        {
            var response = new Response<LoginResponseDto>();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.Message = $"The User with Email{model.Email} does not exist";
                return response;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!signInResult.Succeeded)
            {
                response.Message = "Password validation failed";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;

            //get token
            var token = _tokenService.GenerateToken(user, userRoles, _jwtDataOptions);

            if (string.IsNullOrWhiteSpace(token))
            {
                response.Message = "Token Generation failed";
                return response;
            }

            var loginResponse = new LoginResponseDto()
            {
                Role = string.Join(",", userRoles),
                UserId = user.Id,
                Token = token
            };

            response.Status = true;
            response.Message = "Token Generated successfully";
            response.Data = loginResponse;

            return response;

        }
    }
}
