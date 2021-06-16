using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Users;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Filters;
using UserService.Services.Users;

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
        private readonly ICorporationUsersService _corporationUsersService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CorporationUserController(IMapper mapper, 
            LinkGenerator linkGenerator, ICorporationUsersService corporationUsersService)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _corporationUsersService = corporationUsersService;
        }

        /// <summary>
        /// Returns list of all corporation user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="username">User username</param>
        /// <returns>List of corporation user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<CorporationDto>> GetUsers(string city, string username)
        {
            try {
                var croporationUsers = _corporationUsersService.GetUsers(city, username);
                if (croporationUsers == null || croporationUsers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<CorporationDto>>(croporationUsers));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        /// <summary>
        /// Returns corporation user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Corporation user with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId}")]
        public ActionResult<CorporationDto> GetUserById(Guid userId)
        {
            try
            {
                var croporationUser = _corporationUsersService.GetUserByUserId(userId);
                if (croporationUser == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<CorporationDto>(croporationUser));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //TODO: Provide example body
        /// <summary>
        /// Creates a new corporation user account
        /// </summary>
        /// <param name="corporationUser">Model of corporation user</param>
        /// <returns>Confirmation of the creation of corporation user</returns>
        /// <response code="200">Returns the created corporation user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CorporationUserCreatedConfirmationDto>> CreateUser([FromBody] CorporationUserCreationDto corporationUser)
        {
            try
            {
                
                Corporation userEntity = _mapper.Map<Corporation>(corporationUser);
                CorporationUserCreatedConfirmation userCreated = _corporationUsersService.CreateUser(userEntity, corporationUser.Password).Result;
               

                string location = _linkGenerator.GetPathByAction("GetUserById", "CorporationUser", new { userId = userCreated.UserId });
                return Created(location, _mapper.Map<CorporationUserCreatedConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintViolationException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                if (ex.GetType().IsAssignableFrom(typeof(UniqueValueViolationException)))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);

                }
                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                    switch (ErrorCode)
                    {
                        case 2627:  // Unique constraint error
                            break;
                        case 547:   // Constraint check violation; FK violation
                            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                        case 2601:  // Duplicated key row error; Unique violation
                            return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates corporation user
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="404">Corporation user with userId is not found</response>
        ///<response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">Error on the server while updating</response>
        [Authorize]
        [ServiceFilter(typeof(ResourceOwnerFilter))]
        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CorporationDto> UpdateUser([FromBody] CorporationUserUpdateDto corporationUser, Guid userId)
        {
            try
            {
                var userWithId = _corporationUsersService.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                Corporation corporation = _mapper.Map<Corporation>(corporationUser);
                _corporationUsersService.UpdateUser(corporation, userWithId);

                return Ok(_mapper.Map<CorporationDto>(userWithId));
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintViolationException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                if (ex.GetType().IsAssignableFrom(typeof(UniqueValueViolationException)))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                }
                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                    switch (ErrorCode)
                    {
                        case 2627:  // Unique constraint error
                            break;
                        case 547:   // Constraint check violation; FK violation
                            return StatusCode(StatusCodes.Status422UnprocessableEntity);
                        case 2601:  // Duplicated key row error; Unique violation
                            return StatusCode(StatusCodes.Status409Conflict);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deleting corporation user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">User succesfully deleted</response>
        ///<response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with userId not found</response>
        /// <response code="500">Error on the server while updating</response>
        [Authorize]
        [ServiceFilter(typeof(ResourceOwnerFilter))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                var user = _corporationUsersService.GetUserByUserId(userId);
                if (user == null)
                {
                    return NotFound();
                }
                _corporationUsersService.DeleteUser(userId);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }
    }
}
