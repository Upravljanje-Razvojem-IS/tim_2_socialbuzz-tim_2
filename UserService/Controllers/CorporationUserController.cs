using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;

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

        public CorporationUserController(ICorporationUserRepository corporationUserRepository, IMapper mapper)
        {
            this.corporationUserRepository = corporationUserRepository;
            this.mapper = mapper;
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
    }
}
