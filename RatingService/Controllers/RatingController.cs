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
        private readonly ILoggerRepository<RatingController> logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IAuthService _authService;
        private readonly IMapper mapper;
        public RatingController(IRatingService ratingService,
                                ILoggerRepository<RatingController> logger, LinkGenerator linkGenerator,
                                IAuthService _authService, IMapper mapper)
        {
            _ratingService = ratingService;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this._authService = _authService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Vraca sve postojece ocene na objavama.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva Get All Ratings
        /// GET 'https://localhost:44303/api/rating/' \
        ///     --header 'Authorization: Bearer URIS2021'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <response code="200">Uspesno vracena lista svih ocena na objavama korisnika.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nisu pronadjene ocena ili ne postoji nijedna ocena korisnika na objavama.</response>
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

        /// <summary>
        /// Vraca ocenu na osnovu prosledjenog ID-a.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva Get Rating By ID
        /// GET 'https://localhost:44303/api/rating/ratingID/ratingID' \
        ///     --header 'Authorization: Bearer URIS2021'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="ratingID">ID tipa ocene</param>
        /// <response code="200">Uspesno vracena ocena na osnovu ID-a.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjena nijedna ocena sa zadatim ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Vraca sve ocene na jednoj objavi.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET 'https://localhost:44303/api/rating/user/userID/posts/postsID' \
        /// Primer zahteva koji je uspesan \
        ///     --header 'Authorization: Bearer URIS2021' \
        ///     --param  'PostID = 1' \
        ///     --param  'UserID = 3' \
        /// Primer zahteva koji nije uspesan jer je korisnik sa ID-jem 1 blokirao korisnika sa ID-jem 2, a koji je objavio objavu sa ID-jem 2 \
        ///     --header 'Authorization: Bearer URIS2021' \
        ///     --param  'PostID = 2' \
        ///     --param  'UserID = 1 
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="postID">ID objave</param>
        /// <param name="userID">ID korisnika koji salje zahtev</param>
        /// <response code="200">Uspesno vracena lista ocena na jednoj objavi.</response>
        /// <response code="400">Lose kreiran zahtev, npr. korisnik je blokiran.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("user/{userID}/posts/{postID}")]
        public ActionResult<List<RatingDTO>> GetRatingsByPostID([FromHeader] string key, int postID, int userID) //za nekog usera za njegov post
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Vraca ocene koje je korisnik dobio od drugih.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva Get Rating For User
        /// GET 'https://localhost:44303/api/rating/user/userID' \
        ///     --header 'Authorization: Bearer URIS2021'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="userID">ID korisnika za koga se vracaju ocene</param>
        /// <response code="200">Uspesno vracene ocene.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjena nijedna ocena sa zadatim korisnika ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Vraca sve ocene koje je korisnik ostavio na objavama drugih.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva Get Rating By User
        /// GET 'https://localhost:44303/api/rating/ratingsByUser/userID' \
        ///     --header 'Authorization: Bearer URIS2021'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="userID">ID korisnika za koga se vracaju ocene</param>
        /// <response code="200">Uspesno vracene ocene.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjena nijedna ocena sa zadatim korisnika ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Kreira novu ocenu korisnika na objavi.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="type">Model ocene koja se kreira</param>
        /// <param name="userID">ID korisnika koji salje zahtev za kreiranjem ocene na objavi</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'https://localhost:44303/api/rating/user/userID' \
        /// Primer zahteva za uspesno dodavanje nove ocene \
        ///  --header 'Authorization: Bearer URIS2021' \
        ///  --param 'userID = 2' \
        /// {     \
        ///  "PostID": 3, \
        ///  "ratingTypeID": 2 \
        ///  "ratingDescription": "Very well" \
        /// } \
        /// Primer zahteva za neuspesno dodavanje ocene jer je korisnik vec reagovao na ovu objavu \
        ///  --header 'Authorization: Bearer URIS2021' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "PostID": 1, \
        ///  "ratingTypeID": 7 \
        ///  "ratingDescription": "Only one star" \
        /// }  \
        ///  Primer zahteva za neuspesno dodavanje ocene jer je korisnik sa ID-jem 2 ne prati korisnika sa ID-jem 4, a koji je objavio objavu sa ID-ijem 4 \
        ///  --header 'Authorization: Bearer URIS2021' \
        ///  --param 'UserID = 2' \
        /// {     \
        ///  "PostID": 4, \
        ///  "ratingTypeID": 7 \
        ///  "ratingDescription": "Only one star" \
        /// } 
        /// </remarks>
        /// <response code="201">Vraca kreiranu ocenu na objavi.</response>
        /// <response code="400">Lose kreiran zahtev, npr. korisnik pokusava da doda ocenu na nepostojecu objavu.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("user/{userID}")]
        public ActionResult<RatingDTO> CreateRating([FromHeader] string key, [FromBody] RatingCreationDTO type, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                
                var created = _ratingService.CreateRating(type, userID);


                string location = linkGenerator.GetPathByAction("GetRatingID", "Rating", new { ratingID = created.RatingID });

                return Created(location, created);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating new raiting: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Modifikacija postojece ocene na objavi.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="updatedType">Model ocene koji se modifikuje</param>
        /// <param name="ratingID">ID ocene koja se modifikuje</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za azuriranje ocene  \
        /// PUT 'https://localhost:44303/api/rating/ratingID' \
        ///     --header 'Authorization: Bearer URIS2021'  \
        ///  { \
        /// "ratingID": "7750A8CE-7BEB-457D-B189-08D95B646192", \
        /// "postID": 3, \
        /// "ratingTypeID": 5       \
        /// "ratingDate": "0001-02-03T00:00:00" \
        /// "ratingDescription": "5 zvezdica" \
        /// "userID": 2 \
        ///  }
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspesno izmenjena ocena.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Ne postoji ocena koji korisnik pokusava da modifikuje.</response>
        /// <response code="400">Lose kreiran zahtev, npr. korisnik pokusava da definise ocenu kao tip ocene koji ne postoji.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
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

        /// <summary>
        /// Vrši brisanje jedne ocene korisnika na objavi, na osnovu ID-ja ocene
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za brisanje ocene
        /// DELETE 'https://localhost:44303/api/rating/ratingID' \
        ///     --header 'Authorization: Bearer URIS2021' \
        ///     --param  'ratingID = 7750A8CE-7BEB-457D-B189-08D95B646192'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="ratingID">ID ocene koja se brise</param>
        /// <response code="204">Uspesno obrisana ocena.</response>
        /// <response code="401" > Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Korisnik pokusava da obrise nepostojecu ocenu.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Prikaz HTTP metoda koje korisnik moze da pozove.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za prikaz dostupnih HTTP metoda
        /// OPTIONS 'https://localhost:44303/api/reactions' \
        /// </remarks>
        /// <response code="200">Uspesno prikazane dostupne metode.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetReactionsOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
