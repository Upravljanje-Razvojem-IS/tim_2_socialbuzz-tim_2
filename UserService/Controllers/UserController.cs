using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Users;
using UserService.Entities;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for fetching user accounts
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IPersonalUserRepository personalUserRepository;
        private readonly ICorporationUserRepository corporationUserRepository;
        private readonly IMapper mapper;

        public UserController(IPersonalUserRepository personalUserRepository, ICorporationUserRepository corporationUserRepository,
            IMapper mapper)
        {
            this.personalUserRepository = personalUserRepository;
            this.corporationUserRepository = corporationUserRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns list of all user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="userType">Name of the account type 
        /// (personalUser or corporatitionUser)</param>
        /// <returns>List of user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<UserInfoDto>> GetUsers(string userType, string city, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(userType))
                {
                    List<Corporation> corporations = corporationUserRepository.GetUsers(city, username);
                    List<PersonalUser> personalUsers = personalUserRepository.GetUsers(city, username);
                    List<UserInfoDto> users = new List<UserInfoDto>();
                    users.AddRange(mapper.Map<List<UserInfoDto>>(personalUsers));
                    users.AddRange(mapper.Map<List<UserInfoDto>>(corporations));
                    if (users == null || users.Count == 0)
                    {
                        return NoContent();
                    }
                    return Ok(users);
                }
                else
                {
                    if (string.Equals(userType, "personalUser"))
                    {
                        List<PersonalUser> personalUsers = personalUserRepository.GetUsers(city, username);
                        if (personalUsers == null || personalUsers.Count == 0)
                        {
                            return NoContent();
                        }
                        return Ok(mapper.Map<List<UserInfoDto>>(personalUsers));
                    }
                    else if (string.Equals(userType, "corporationUser"))
                    {
                        List<Corporation> corporations = corporationUserRepository.GetUsers(city, username);
                        if (corporations == null || corporations.Count == 0)
                        {
                            return NoContent();
                        }
                        return Ok(mapper.Map<List<UserInfoDto>>(corporations));
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Returns user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns> User with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId}")]
        public ActionResult<UserInfoDto> GetUserById(Guid userId)
        {
            try
            {
                var coprporationUser = corporationUserRepository.GetUserByUserId(userId);
                if (coprporationUser != null)
                {
                    return Ok(mapper.Map<UserInfoDto>(coprporationUser));
                }
                var personalUser = personalUserRepository.GetUserByUserId(userId);
                if (personalUser != null)
                {
                    return Ok(mapper.Map<UserInfoDto>(personalUser));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
    }
}
