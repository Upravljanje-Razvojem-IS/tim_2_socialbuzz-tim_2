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

        /// <summary>
        /// Vraca sve postojece tipove ocena nad objavama.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get All Types Of Reaction
        /// GET 'https://localhost:44303/api/ratingtypes/' \
        ///     --header 'Authorization: Bearer URIS2021' 
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <response code="200">Uspesno vracena lista svih tipova ocena nad objavama korisnika.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nisu pronadjeni tipovi ocena ili ne postoji nijedan tip ocena korisnika.</response>
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

        /// <summary>
        /// Vraca tip ocene na osnovu prosledjenog ID-a.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get RatingType By ID
        /// GET 'https://localhost:44303/api/ratingtypes/ratingTypeID' \
        ///     --header 'Authorization: Bearer URIS2021'  \
        ///     --url  'ratingTypeID = 1'  
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="ratingTypeID">ID tipa reakcije</param>
        /// <response code="200">Uspesno vracen tip ocene na osnovu ID-a.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjen nijedan tip ocene sa zadatim ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Kreira novi tip ocene.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="type">Model ocene koja se kreira</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva za dodavanje novog tipa ocene \
        /// POST 'https://localhost:44303/api/ratingtypes/' \
        ///      --header 'Authorization: Bearer URIS2021' \
        /// {   \
        ///  "RatingTypeName": "Very bad", \
        /// }
        /// </remarks>
        /// <response code="201">Vraca kreirani tip ocene.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        public ActionResult<RatingTypeDTO> CreateRatingType([FromHeader] string key, [FromBody] RatingTypeCreationDTO type)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
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

        /// <summary>
        /// Modifikacija postojeceg tipa ocene.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="updatedType">Model tipa ocene koji se modifikuje</param>
        /// <param name="typeID">ID tipa ocene koji se modifikuje</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva za azuriranje tipa ocene \
        /// PUT  'https://localhost:44303/api/ratingtypes/typeID' \
        ///    --header 'Authorization: Bearer URIS2021' \
        /// { \
        /// "RatingTypeID": 1, \
        /// "RatingTypeName": "Updating excellent", \
        /// }
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspesno izmenjen tip ocene.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Ne postoji tip ocene sa datim ID-ijem a koji korisnik pokusava da modifikuje.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
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
                logger.LogError(ex, "Error updating type of rating: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Vrši brisanje jednog tipa ocene, na osnovu ID-ja tipa ocene
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za brisanje tipa ocene
        /// DELETE 'https://localhost:44303/api/ratingtypes/ratingTypeID' \
        ///     --header 'Authorization: Bearer URIS2021' \
        ///     --param  'ratingTypeID = 8'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="ratingTypeID">ID tipa ocene koji se brise</param>
        /// <response code="204">Uspesno obrisan tip ocene.</response>
        /// <response code="401"> Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Korisnik pokusava da obrise nepostojeci tip ocene.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Prikaz HTTP metoda koje korisnik moze da pozove.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za prikaz dostupnih HTTP metoda
        /// OPTIONS 'https://localhost:44303/api/ratingtypes' \
        /// </remarks>
        /// <response code="200">Uspesno prikazane dostupne metode.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetRatingTypesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
