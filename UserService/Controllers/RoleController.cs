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
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Roles;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Services.Roles;

//TODO: RoleManager 
namespace UserService.Controllers
{
    /// <summary>
    /// Controller with endpoints for fetching, creating and deleting roles
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly RoleManager<AccountRole> _roleManager;

        public RoleController(IRoleRepository roleRepository, IMapper mapper, LinkGenerator linkGenerator, IRolesService rolesService,
            RoleManager<AccountRole> roleManager)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _rolesService = rolesService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns list of all roles in the system
        /// </summary>
        /// <param name="roleName"> Name of the role </param>
        /// <returns>List of roles</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No roles  are found</response>
        ///<response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<RoleDto>> GetRoles(string roleName)
        {
            try
            {
                var roles = _rolesService.GetRoles(roleName);
                if (roles == null || roles.Count == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<RoleDto>>(roles));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Returns role with roleId
        /// </summary>
        /// <param name="roleId">User's Id</param>
        /// <returns>Role with roleId</returns>
        ///<response code="200">Returns the role</response>
        /// <response code="404">Role with roleId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [HttpGet("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RoleDto> GetRoleById(Guid roleId)
        {
            try
            {
                var role = _rolesService.GetRoleByRoleId(roleId);
                if (role == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RoleDto>(role));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        //TODO: Provide example body
        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="city">Model of role</param>
        /// <returns>Confirmation of the creation of role</returns>
        /// <response code="200">Returns the created role</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="409">Unique value violation</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public ActionResult<RoleCreatedConfirmationDto> CreateRole([FromBody] RoleCreationDto role)
        {
            try
            {
                Role newRole = _mapper.Map<Role>(role);
                RoleCreatedConfirmation createdRole = _rolesService.CreateRole(newRole);
                var location = _linkGenerator.GetPathByAction("GetRoleById", "Role", new { roleId = createdRole.RoleId });
                return Created(location, _mapper.Map<RoleCreatedConfirmationDto>(createdRole));
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().GetType() == typeof(UniqueValueViolationException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Updates role
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated role</response>
        /// <response code="400">Role with roleId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="409">Unique value violation</response>
        /// <response code="500">Error on the server while updating</response>
        [HttpPut("{roleId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<RoleDto> UpdateRole([FromBody] RoleUpdateDto roleUpdate, Guid roleId)
        {
            try
            {
                Role roleWithId = _rolesService.GetRoleByRoleId(roleId);
                if (roleWithId == null)
                {
                    return NotFound();
                }
                if (_roleManager.RoleExistsAsync(roleUpdate.RoleName).Result)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Role name should be unique");

                }
                var role = _roleManager.Roles.First(r => r.Id == roleId);
                role.Name = roleUpdate.RoleName;
                _roleManager.UpdateAsync(role).Wait();

                Role updatedRole = _mapper.Map<Role>(roleUpdate);
                updatedRole.RoleId = roleId;
                _mapper.Map(updatedRole, roleWithId);
                _roleRepository.SaveChanges();
                return Ok(_mapper.Map<RoleDto>(roleWithId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        /// <summary>
        /// Deleting role with roleId
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Role succesfully deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Role with roleId not found</response>
        /// <response code="409">Role reference in another table</response>
        /// <response code="500">Error on the server while deleting</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{roleId}")]
        public IActionResult DeleteRole(Guid roleId)
        {
            try
            {
                var role = _roleRepository.GetRoleByRoleId(roleId);
                if (role == null)
                {
                    return NotFound();
                }
                _rolesService.DeleteRole(roleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintViolationException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }


    }
}
