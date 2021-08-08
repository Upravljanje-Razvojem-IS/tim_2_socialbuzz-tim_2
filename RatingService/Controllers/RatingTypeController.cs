using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RatingService.Auth;
using RatingService.DTO;
using RatingService.Entities;
using RatingService.Logger;
using RatingService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    /// <summary>
    /// RatingType Kontroler izvrsava CRUD operacije nad podacima />.
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/ratingtypes")]
    [ApiController]
    public class RatingTypeController : ControllerBase
    {
        private readonly IRatingTypeService _ratingTypeService;
        private readonly ILoggerRepository<RatingTypeController> logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IAuthService _authService;
        public RatingTypeController(IRatingTypeService ratingTypeService,
                                ILoggerRepository<RatingTypeController> logger, LinkGenerator linkGenerator, IMapper mapper,
                                IAuthService authService)
        {
            _ratingTypeService = ratingTypeService;
            _authService = authService;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<List<RatingTypeDTO>> GetAllRatingTypes([FromHeader] string key)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }
            
            var types = _ratingTypeService.GetAllRatingTypes();

            if (types == null || types.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "No types of rating found...");
            }

            logger.LogInformation("Successfully returned list of all rating types.");

            return Ok(types);
        }

        
        [HttpGet("{ratingTypeID}")]
        public ActionResult GetRatingTypeByID([FromHeader] string key, int ratingTypeID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var type = _ratingTypeService.GetRatingTypeByID(ratingTypeID);
                if (type == null)
                {
                    return NotFound();
                }

                logger.LogInformation("Successfully returned type of reaction based on ID");

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<RatingTypeDTO> CreateRatingType([FromHeader] string key, [FromBody] RatingTypeCreationDTO type)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                RatingType typeEntity = mapper.Map<RatingType>(type);
                var createdType = _ratingTypeService.CreateRatingType(type);

                string location = linkGenerator.GetPathByAction("GetRatingTypeByID", "RatingType", new { ratingTypeID = createdType.RatingTypeID });

                return Created(location, createdType);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating new type of raiting: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new type of rating!");
            }

        }

        [HttpPut("{typeID}")]
        public IActionResult UpdateRatingType([FromHeader] string key, [FromBody] RatingTypeModifyingDTO updatedType, int typeID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            var newType = mapper.Map<RatingType>(updatedType);

            try
            {
                _ratingTypeService.UpdateRatingType(updatedType, typeID);
                var res = mapper.Map<RatingType>(newType);
                res.RatingTypeID = typeID;

                return Ok(res);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating type of reaction: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating type of reaction");

            }
        }

        [HttpDelete("{ratingTypeID}")]
        public IActionResult DeleteTypeOfReaction( [FromHeader] string key,int ratingTypeID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                _ratingTypeService.DeleteRatingType(ratingTypeID);

                return NoContent();
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting type of reaction: " + ex.Message);
                if(ex.Message.Contains("There is no rating type with that ID!"))
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting type of reaction!");
            }
        }

    }
}
