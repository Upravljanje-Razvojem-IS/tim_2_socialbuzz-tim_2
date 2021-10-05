using ImageMicroservice.Dto;
using ImageMicroservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }
        /// <summary>
        /// Logovanje korisnika
        /// </summary>
        /// <param name="login">Podaci korisnika.</param>
        /// <returns>Potvrdu o kreiranju i token</returns>
        /// <remarks>
        /// POST http://localhost:44315/user/id
        /// </remarks>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                string tokenString =_auth.Login(login.Username, login.Password);
                return Ok(new { Token = tokenString });
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
