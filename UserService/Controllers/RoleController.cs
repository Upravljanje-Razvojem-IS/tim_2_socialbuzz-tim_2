﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Dtos;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        [HttpGet]
        public  ActionResult<CityDto> GetRoles()
        {
            return Ok(null);
        }
     
    }
}