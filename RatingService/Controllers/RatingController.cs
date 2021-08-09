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
using System.Net;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    /// <summary>
    /// Rating Kontroler izvrsava CRUD operacije nad podacima />.
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IRatingTypeService _ratingTypeService;
        private readonly ILoggerRepository<RatingController> logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IAuthService _authService;
        private readonly IMapper mapper;
        public RatingController(IRatingService ratingService, IRatingTypeService ratingTypeService,
                                ILoggerRepository<RatingController> logger, LinkGenerator linkGenerator,
                                IAuthService _authService, IMapper mapper)
        {
            _ratingService = ratingService;
            _ratingTypeService = ratingTypeService;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this._authService = _authService;
            this.mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [HttpHead]
        public ActionResult<List<RatingDTO>> GetAllRatings([FromHeader] string key)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }
            
            var ratings = _ratingService.GetAllRatings();

            if (ratings == null || ratings.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "No raitings found...");
            }

            logger.LogInformation("Successfully returned list of all ratings.");

            return Ok(ratings);
        }

        [HttpGet("ratingID/{ratingID}")]
        public ActionResult<RatingDTO> GetRatingID([FromHeader] string key, Guid ratingID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var ratings = _ratingService.GetRatingByID(ratingID);

                logger.LogInformation("Successfully returned list of all ratings on a single post.");

                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("user/{userID}/posts/{postID}")]
        public ActionResult<List<RatingDTO>> GetRatingsByPostID([FromHeader] string key, int postID,int userID) //za nekog usera za njegov post
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var ratings = _ratingService.GetRatingByPostID(postID, userID);

                logger.LogInformation("Successfully returned list of all ratings on a single post.");

                return Ok(ratings);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("user/{userID}")]
        public ActionResult<List<RatingDTO>> GetRatingsForUser([FromHeader] string key, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var ratings = _ratingService.GetAllRatingsForUser(userID);

                logger.LogInformation("Successfully returned list of all ratings user recieved.");

                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("ratingsByUser/{userID}")]
        public ActionResult<List<RatingDTO>> GetRatingsByUser([FromHeader] string key, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var ratings = _ratingService.GetAllRatingsByUser(userID);

                logger.LogInformation("Successfully returned list of all ratings user gave.");

                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("user/{userID}")]
        public ActionResult<RatingTypeDTO> CreateRating([FromHeader] string key, [FromBody] RatingCreationDTO type, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                Rating entity = mapper.Map<Rating>(type);
                var created = _ratingService.CreateRating(type, userID);

                //created.UserID = userID;

                string location = linkGenerator.GetPathByAction("GetRatingID", "Rating", new { ratingID = created.RatingID });

                return Created(location, created);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating new raiting: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("{ratingID}")]
        public IActionResult UpdateRating([FromHeader] string key, [FromBody] RatingModifyingDTO updatedType, Guid ratingID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            var newType = mapper.Map<Rating>(updatedType);

            try
            {
                _ratingService.UpdateRating(updatedType, ratingID);
                var res = mapper.Map<Rating>(newType);
                res.RatingID = ratingID;

                return Ok(res);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating rating: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpDelete("{ratingID}")]
        public IActionResult DeleteTypeOfReaction([FromHeader] string key, Guid ratingID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                _ratingService.DeleteRating(ratingID);

                return NoContent();
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting rating: " + ex.Message);

                if (ex.Message.Contains("There is no rating with that ID!"))
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting rating!");
            }
        }

    }
}
