using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IRoleRepository roleRepository;
        private readonly ICityRepository cityRepository;
        private readonly IPersonalUserRepository personalUserRepository;

        public CorporationUserController(ICorporationUserRepository corporationUserRepository, IMapper mapper, 
            LinkGenerator linkGenerator, IRoleRepository roleRepository, ICityRepository cityRepository, IPersonalUserRepository personalUserRepository)
        {
            this.corporationUserRepository = corporationUserRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.roleRepository = roleRepository;
            this.cityRepository = cityRepository;
            this.personalUserRepository = personalUserRepository;
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
                var user = personalUserRepository.GetUsers(null, corporationUser.Username);
                if (user != null && user.Count > 0)
                {
                    //Unique violation
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                Corporation userEntity = mapper.Map<Corporation>(corporationUser);
                CorporationUserCreatedConfirmation userCreated = corporationUserRepository.CreateUser(userEntity);
                corporationUserRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetUserById", "CorporationUser", new { userId = userCreated.UserId });

                return Created(location, mapper.Map<CorporationUserCreatedConfirmationDto>(userCreated));
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
        /// Updates corporation user
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="400">Corporation user with userId is not found</response>
        /// <response code="500">Error on the server while updating</response>
        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CorporationDto> UpdateUser([FromBody] CorporationUserUpdateDto corporationUser, Guid userId)
        {
            try
            {
                var userWithId = corporationUserRepository.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                var user = personalUserRepository.GetUsers(null, corporationUser.Username);
                if (user != null && user.Count > 0)
                {
                    //Unique violation
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                //TODO: Role can be changed only by admin, PATCH 
                //TODO: Cleaner code
                //TODO: Bad foreign keys, unique
                //TODO: Password change PATCH?
                Corporation updatedUser = mapper.Map<Corporation>(corporationUser);
                updatedUser.RoleId = userWithId.RoleId;
                updatedUser.Role = roleRepository.GetRoleByRoleId(userWithId.RoleId);
                updatedUser.City = cityRepository.GetCityByCityId(updatedUser.CityId);
                //If updated.City is null FK violation
                updatedUser.UserId = userId;
                mapper.Map(updatedUser, userWithId);
                corporationUserRepository.SaveChanges();
                return Ok(mapper.Map<CorporationDto>(userWithId));
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
                var user = corporationUserRepository.GetUserByUserId(userId);
                if (user == null)
                {
                    return NotFound();
                }
                corporationUserRepository.DeleteUser(userId);
                corporationUserRepository.SaveChanges();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }
    }
}
