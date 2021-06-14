using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authService;

        public AuthController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost("api/login")]
        public IActionResult Register([FromBody] Principal principal)
        {
            var authResponse = authService.Login(principal);
            if (authResponse.Result.Succes)
            {
                return Ok(new AuthSuccesResponse
                {
                    Token = authResponse.Result.Token
                });
            }
            return BadRequest(new AuthFailResponse
            {
                Error = authResponse.Result.Error
            });
        }
    }
}
