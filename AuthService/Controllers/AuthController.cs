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

        [HttpGet("api/register")]
        public async Task<IActionResult> Register([FromBody] Principal principal)
        {
            var authResponse = await authService.LoginAsync(principal.Email, principal.Password);
            return Ok();
        }
    }
}
