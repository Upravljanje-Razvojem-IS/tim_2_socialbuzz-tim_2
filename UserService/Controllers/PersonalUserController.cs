using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Users;
using UserService.Entities;

namespace UserService.Controllers
{
    /// <summary>
    /// Contoller with endopoints for fetching, creating, updating
    /// and deleting personal user accounts
    /// </summary>
    [ApiController]
    [Route("api/personalUsers")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IPersonalUserRepository personalUserRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IRoleRepository roleRepository;
        private readonly ICityRepository cityRepository;
        private readonly ICorporationUserRepository corporationUserRepository;

        public PersonalUserController(IPersonalUserRepository personalUserRepository, IMapper mapper, 
            LinkGenerator linkGenerator, IRoleRepository roleRepository, ICityRepository cityRepository, ICorporationUserRepository corporationUserRepository)
        {
            this.personalUserRepository = personalUserRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.roleRepository = roleRepository;
            this.cityRepository = cityRepository;
            this.corporationUserRepository = corporationUserRepository;
        }

        /// <summary>
        /// Returns list of all personal user accounts in the system
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <returns>List of personal user accounts</returns>
        /// <response code="200"> the list</response>
        /// <response code="204">No user accounts are found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<PersonalUserDto>> GetUsers(string city, string username)
        {
            var personalUsers = personalUserRepository.GetUsers(city, username);
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
        /// <response code="404">User with userId is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}")]
        public ActionResult<PersonalUserDto> GetUserById(Guid userId)
        {
            var personalUser = personalUserRepository.GetUserByUserId(userId);
            if (personalUser == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PersonalUserDto>(personalUser));
        }


        //TODO: Provide example body
        /// <summary>
        /// Creates a new personal user account
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatedConfirmationDto> CreateUser([FromBody] PersonalUserCreationDto personalUser)
        {
            try
            {
                var user = corporationUserRepository.GetUsers(null, personalUser.Username);
                if (user != null && user.Count > 0)
                {
                    //Unique violation
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                PersonalUser userEntity = mapper.Map<PersonalUser>(personalUser);
                PersonalUserCreatedConfirmation userCreated = personalUserRepository.CreateUser(userEntity);
                personalUserRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = userCreated.UserId });

                return Created(location, mapper.Map<PersonalUserCreatedConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
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
        /// <response code="400">Personal user with userId is not found</response>
        /// <response code="500">Error on the server while updating</response>
        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserDto> UpdateUser([FromBody] PersonalUserUpdateDto personalUser, Guid userId)
        {
            try
            {
                var userWithId = personalUserRepository.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                //TODO: Role can be changed only by admin, PATCH 
                //TODO: Cleaner code
                //TODO: Bad foreign keys, unique
                //TODO: Password change PATCH?
                var user = corporationUserRepository.GetUsers(null, personalUser.Username);
                if (user != null && user.Count > 0)
                {
                    //Unique violation
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                PersonalUser updatedUser = mapper.Map<PersonalUser>(personalUser);
                updatedUser.RoleId = userWithId.RoleId;
                updatedUser.RoleId = userWithId.RoleId;
                updatedUser.Role = roleRepository.GetRoleByRoleId(userWithId.RoleId);
                updatedUser.City = cityRepository.GetCityByCityId(updatedUser.CityId);
                updatedUser.UserId = userId;
                mapper.Map(updatedUser, userWithId);
                personalUserRepository.SaveChanges();
                return Ok(mapper.Map<PersonalUserDto>(userWithId));
            }
            catch(Exception ex)
            {
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
        /// <response code="404">User with userId not found</response>
        /// <response code="500">Error on the server while deleting</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                var user = personalUserRepository.GetUserByUserId(userId);
                if (user == null)
                {
                    return NotFound();
                }
                personalUserRepository.DeleteUser(userId);
                personalUserRepository.SaveChanges();
                return NoContent();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }

        //TODO: Provide example body
        /// <summary>
        /// Creates a new personal user account
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost("admins")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatedConfirmationDto> CreateAdmin([FromBody] PersonalUserCreationDto personalUser)
        {
            try
            {
                var user = corporationUserRepository.GetUsers(null, personalUser.Username);
                if (user != null && user.Count >0)
                {
                    //Unique violation
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                PersonalUser userEntity = mapper.Map<PersonalUser>(personalUser);
                PersonalUserCreatedConfirmation userCreated = personalUserRepository.CreateAdmin(userEntity);
                personalUserRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = userCreated.UserId });

                return Created(location, mapper.Map<PersonalUserCreatedConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
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
