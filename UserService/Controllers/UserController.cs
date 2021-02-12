using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Dtos;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for fetching, creating, updating
    /// and deleting user accounts
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public  ActionResult<UserDto> GetUsers()
        {
            return Ok(null);
        }
     
    }
}
