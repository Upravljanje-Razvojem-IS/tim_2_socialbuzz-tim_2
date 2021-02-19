using System;
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
    /// Controller with endpoints for fetching, creating and deleting roles
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns list of all roles in the system
        /// </summary>
        /// <returns>List of roles</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No roles  are found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<CityDto>> GetRoles()
        {
            var roles = roleRepository.GetRoles();
            if (roles == null || roles.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<CityDto>>(roles));
        }

        /// <summary>
        /// Returns role with roleId
        /// </summary>
        /// <param name="roleId">User's Id</param>
        /// <returns>Role with roleId</returns>
        ///<response code="200">Returns the role</response>
        /// <response code="404">Role with roleId is not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CityDto> GetRoleById(Guid roleId)
        {
            var role = roleRepository.GetRoleByRoleId(roleId);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CityDto>(role));
        }
    }
}
