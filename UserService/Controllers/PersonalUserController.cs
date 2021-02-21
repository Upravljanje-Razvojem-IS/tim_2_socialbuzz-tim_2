using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

        public PersonalUserController(IPersonalUserRepository personalUserRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.personalUserRepository = personalUserRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
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


        //TODO: Provide exapmple body
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
                PersonalUser userEntity = mapper.Map<PersonalUser>(personalUser);
                PersonalUserCreatedConfirmation userCreated = personalUserRepository.CreateUser(userEntity);
                personalUserRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = userCreated.UserId });

                return Created(location, mapper.Map<PersonalUserCreatedConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        } 
    }
}
