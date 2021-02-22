﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Users;
using UserService.Entities;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for fetching, creating, updating
    /// and deleting corporate user accounts
    /// </summary>
    [ApiController]
    [Route("api/corporationUsers")]
    public class CorporationUserController : ControllerBase
    {
        private readonly ICorporationUserRepository corporationUserRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CorporationUserController(ICorporationUserRepository corporationUserRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.corporationUserRepository = corporationUserRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Returns list of all corporation user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <returns>List of corporation user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<CorporationDto>> GetUsers(string city, string username)
        {
            var croporationUsers = corporationUserRepository.GetUsers(city, username);
            if (croporationUsers == null || croporationUsers.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<CorporationDto>>(croporationUsers));
        }

        /// <summary>
        /// Returns corporation user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Corporation user with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="404">User with userId is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}")]
        public ActionResult<CorporationDto> GetUserById(Guid userId)
        {
            var croporationUser = corporationUserRepository.GetUserByUserId(userId);
            if (croporationUser == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CorporationDto>(croporationUser));
        }

        //TODO: Provide example body
        /// <summary>
        /// Creates a new corporation user account
        /// </summary>
        /// <param name="corporationUser">Model of corporation user</param>
        /// <returns>Confirmation of the creation of corporation user</returns>
        /// <response code="200">Returns the created corporation user</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CorporationUserCreatedConfirmationDto> CreateUser([FromBody] CorporationUserCreationDto corporationUser)
        {
            try
            {
                Corporation userEntity = mapper.Map<Corporation>(corporationUser);
                CorporationUserCreatedConfirmation userCreated = corporationUserRepository.CreateUser(userEntity);
                corporationUserRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetUserById", "CorporationUser", new { userId = userCreated.UserId });

                return Created(location, mapper.Map<CorporationUserCreatedConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
