using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Application.Dtos;
using TrustBank.Application.Responses;
using TrustBank.Application.Services.Interfaces;
using TrustBank.Core.Models;

namespace TrustBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<Customer> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginDto model)
        {
           return Ok(await _authService.Login(model));
        }
    }
}
