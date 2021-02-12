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
    /// Controller with endpoints for fetching cities
    /// </summary>
    [ApiController]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        [HttpGet]
        public  ActionResult<CityDto> GetCities()
        {
            return Ok(null);
        }
     
    }
}
