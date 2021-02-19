﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Dtos;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for creating, updating
    /// and deleting personal user accounts
    /// </summary>
    [ApiController]
    [Route("api/personalUsers")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IPersonalUserRepository personalUserRepository;
        private readonly IMapper mapper;

        public PersonalUserController(IPersonalUserRepository personalUserRepository, IMapper mapper)
        {
            this.personalUserRepository = personalUserRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns list of all personal user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <returns>List of personal user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<PersonalUserDto>> GetUsers(string city)
        {
            var personalUsers = personalUserRepository.GetUsers(city);
            if (personalUsers == null || personalUsers.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<PersonalUserDto>>(personalUsers));
        }

        /// <summary>
        /// Returns personal user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Personal user with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="204">User with userId is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet("{userId}")]
        public ActionResult<PersonalUserDto> GetUserById(Guid userId)
        {
            var personalUser = personalUserRepository.GetUserByUserId(userId);
            if (personalUser == null)
            {
                return NoContent();
            }
            return Ok(mapper.Map<PersonalUserDto>(personalUser));
        }

    }
}
