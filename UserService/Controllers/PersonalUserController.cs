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
using UserService.Extensions;
using UserService.Filters;
using UserService.Services;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for fetching, creating, updating
    /// and deleting personal user accounts
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/personalUsers")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IPersonalUsersService _personalUsersService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public PersonalUserController(IMapper mapper, 
            LinkGenerator linkGenerator, IPersonalUsersService personalUsersService)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _personalUsersService = personalUsersService;
        }

        /// <summary>
        /// Returns list of all personal user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="username">User username</param>
        /// <returns>List of personal user accounts</returns>
        /// <response code="200"> the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<PersonalUserDto>> GetUsers(string city, string username)
        {
            try{
                var personalUsers = _personalUsersService.GetUsers(city, username);
                if (personalUsers == null || personalUsers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<PersonalUserDto>>(personalUsers));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Returns personal user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Personal user with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}")]
        public ActionResult<PersonalUserDto> GetUserById(Guid userId)
        {
            try
            {
                var personalUser = _personalUsersService.GetUserByUserId(userId);
                if (personalUser == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PersonalUserDto>(personalUser));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
           
        }


        //TODO: Provide example body
        /// <summary>
        /// Creates a new personal user account
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatedConfirmationDto> CreateUser([FromBody] PersonalUserCreationDto personalUser)
        {
            try
            {
                PersonalUser userEntity = _mapper.Map<PersonalUser>(personalUser);

                PersonalUserCreatedConfirmation userCreated = _personalUsersService.CreateUser(userEntity, personalUser.Password);

                string location = _linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = userCreated.UserId });

                return Created(location, _mapper.Map<PersonalUserCreatedConfirmationDto>(userCreated));
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
        /// Updates personal user
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="404">Personal user with userId is not found</response>
        /// <response code="500">Error on the server while updating</response>
        /// <response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        [Authorize]
        [ServiceFilter(typeof(ResourceOwnerFilter))]
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserDto> UpdateUser([FromBody] PersonalUserUpdateDto personalUser, Guid userId)
        {
            try
            {
                var userWithId = _personalUsersService.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                PersonalUser user = _mapper.Map<PersonalUser>(personalUser);
                _personalUsersService.UpdateUser(user, userWithId);

                return Ok(_mapper.Map<PersonalUserDto>(userWithId));
            }
            catch(Exception ex)
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
        /// Deleting personal user with userId
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">User succesfully deleted</response>
        /// <response code="400">User doesn't own the resource</response>
        /// <response code="404">User with userId not found</response>
        /// <response code="500">Error on the server while deleting</response>
        [Authorize]
        [ServiceFilter(typeof(ResourceOwnerFilter))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                var user = _personalUsersService.GetUserByUserId(userId);
                if (user == null)
                {
                    return NotFound();
                }
                _personalUsersService.DeleteUser(userId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //TODO: Provide example body
        /// <summary>
        /// Creates a new personal user account
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        /// <response code="401">Unauthorize user</response>
        /// <response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize(Roles="Admin")]
        [HttpPost("admins")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatedConfirmationDto> CreateAdmin([FromBody] PersonalUserCreationDto personalUser)
        {
            try
            {
                PersonalUser userEntity = _mapper.Map<PersonalUser>(personalUser);

                //Adding to userdbcontext tables
                PersonalUserCreatedConfirmation userCreated = _personalUsersService.CreateAdmin(userEntity, personalUser.Username);

                string location = _linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = userCreated.UserId });
                return Created(location, _mapper.Map<PersonalUserCreatedConfirmationDto>(userCreated));
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


    }
}
