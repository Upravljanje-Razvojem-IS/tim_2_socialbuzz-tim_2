using Microsoft.AspNetCore.Mvc;
using UserWatchingService.Entities;
using UserWatchingService.Interfaces;

namespace UserWatchingService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>token</returns>
        [HttpPost]
        public ActionResult Login([FromBody] Principal principal)
        {
            string token = _authService.Login(principal);

            return Ok(new { Token = token });
        }
    }
}
