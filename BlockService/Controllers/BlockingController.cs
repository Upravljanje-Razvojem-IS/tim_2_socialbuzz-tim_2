using AutoMapper;
using BlockService.Auth;
using BlockService.DTO;
using BlockService.Entities;
using BlockService.Logger;
using BlockService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Controllers
{
    /// <summary>
    /// Block Kontroler izvrsava CRUD operacije nad podacima />.
    /// </summary>
    [Produces("application/json", "application/xml")]
    [Route("api/block")]
    [ApiController]
    public class BlockingController : ControllerBase
    {
        private readonly IBlockingService _blockingService;
        private readonly ILoggerRepository<BlockingController> logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IAuthService _authService;
        private readonly IMapper mapper;
        public BlockingController(IBlockingService blockingService, LinkGenerator linkGenerator,
                                  IMapper mapper, IAuthService authService, ILoggerRepository<BlockingController> logger)
        {
            _blockingService = blockingService;
            _authService = authService;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Vraca sve postojeca blokiranja korisnika.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get All Blocks
        /// GET 'http://localhost:4207/api/block/' \
        ///     --header 'Authorization: Bearer URIS2021' 
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <response code="200">Uspesno vracena lista svih blokiranja korisnika.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nisu pronadjena blokiranja ili ne postoji nijedno blokiranje korisnika.</response>
        [HttpGet]
        [HttpHead]
        public ActionResult<List<BlockDTO>> GetAllBlocks([FromHeader] string key)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            var types = _blockingService.GetAllBlocks();

            if (types == null || types.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "No blocks found...");
            }

            logger.LogInformation("Successfully returned list of blocks.");

            return Ok(types);
        }

        /// <summary>
        /// Vraca blokiranje na osnovu prosledjenog ID-a.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get Block By ID
        /// GET 'http://localhost:4207/api/block/blockID' \
        ///     --header 'Authorization: Bearer URIS2021'  \
        ///     --url  'blockID = 8CA02E0F-A565-43D7-B8D1-DA0A073118FB'  
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="blockID">ID blokiranja</param>
        /// <response code="200">Uspesno vraceno blokiranje na osnovu ID-a.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjeno nijedno blokiranje sa zadatim ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("blockID/{BlockID}")]
        public ActionResult GetBlockByID([FromHeader] string key, Guid BlockID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var type = _blockingService.GetBlockById(BlockID);
                if (type == null)
                {
                    return NotFound();
                }

                logger.LogInformation("Successfully returned block based on ID");

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Vraca sve one koje je korisnik, sa prosledjenim ID-em, blokirao.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get Blocks By User
        /// GET 'http://localhost:4207/api/block/userBlocks/userID' \
        ///     --header 'Authorization: Bearer URIS2021'  \
        ///     --url  'userID = 1'  
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="userID">ID korisnika</param>
        /// <response code="200">Uspesno vracena lista blokova za korisnika na osnovu ID-a.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjen nijedan blok za korisnika.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("userBlocks/{userID}")]
        public ActionResult<List<BlockDTO>> GetBlocksByUser([FromHeader] string key, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var type = _blockingService.GetBlocksByUser(userID);
                if (type == null)
                {
                    return NotFound();
                }

                logger.LogInformation("Successfully returned block based on ID");

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Vraca listu blokova, odnosno ko je sve blokirao datog korisnika.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva Get Blocks For User
        /// GET 'http://localhost:4207/api/block/blocksForUser/userID' \
        ///     --header 'Authorization: Bearer URIS2021'  \
        ///     --url  'userID = 1'  
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="userID">ID korisnika</param>
        /// <response code="200">Uspesno vraca ko je sve blokirao korisnika, na osnovu ID-a.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Nije pronadjen nijedan blok za korisnika sa zadatim ID-jem.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("blocksForUser/{userID}")]
        public ActionResult<List<BlockDTO>> GetBlocksForUser([FromHeader] string key, int userID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var type = _blockingService.GetBlocksForUser(userID);
                if (type == null)
                {
                    return NotFound();
                }

                logger.LogInformation("Successfully returned block based on ID");

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kreira novi blok, odnosno dozvoljava korisniku da blokira drugoga.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="block">Model blokiranja koji se dodaje</param>
        /// <param name="blockerID">Korisnik koji blokira</param>
        /// <param name="blockedID">Korisnik koji se blokira</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva za dodavanje novog bloka \
        /// POST 'http://localhost:4207/api/block/blockerID/blockerID/blockedID/blockedID' \
        ///      --header 'Authorization: Bearer URIS2021' \
        /// {   \
        ///  "blockerID": 3, \
        ///  "blockedID": 2 \
        /// }
        /// </remarks>
        /// <response code="201">Vraca kreirano blokiranje.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("blockerID/{blockerID}/blockedID/{blockedID}")]
        public ActionResult<BlockDTO> Block([FromHeader] string key, [FromBody] BlockCreationDTO block, int blockerID, int blockedID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                var createdType = _blockingService.Block(block,  blockerID,  blockedID);
                

                //string location = linkGenerator.GetPathByAction("GetBlockByID", "Block", new { BlockID = createdType.BlockID });

                return StatusCode(StatusCodes.Status201Created, "You have successfully blocked user");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating new block: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Modifikacija postojeceg blokiranja.
        /// </summary>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="updated">Model blokiranja koji se modifikuje</param>
        /// <param name="BlockID">ID blokiranja koji se modifikuje</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer uspesnog zahteva za azuriranje tipa ocene \
        /// PUT  'http://localhost:4207/api/block/BlockID' \
        ///    --header 'Authorization: Bearer URIS2021' \
        ///    --url  'blockID = 8CA02E0F-A565-43D7-B8D1-DA0A073118FB'
        /// { \
        /// "blockerID": 3,\
        /// "blockedID": 2, \
        /// }
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspesno izmenjen blok.</response>
        /// <response code="401">Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Ne postoji blok sa datim ID-ijem a koji korisnik pokusava da modifikuje.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("{BlockID}")]
        public IActionResult UpdateRatingType([FromHeader] string key, [FromBody] BlockModifyingDTO updated, Guid BlockID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            var newType = mapper.Map<Block>(updated);

            try
            {
                _blockingService.UpdateBlock(updated, BlockID);
                var res = mapper.Map<Block>(newType);
                res.BlockID = BlockID;

                return Ok(res);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating block: " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Vrši brisanje jednog blokiranja, na osnovu ID-ja blokiranja
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za brisanje 
        /// DELETE 'http://localhost:4207/api/block/blockerID/blockerID/blockedID/blockedID' \
        ///     --header 'Authorization: Bearer URIS2021' \
        ///     --param  'blockerID = 3'
        ///     --param  'blockedID = 2'
        /// </remarks>
        /// <param name="key">Authorization Header Bearer Key Value</param>
        /// <param name="blockerID">Korisnik koji blokira</param>
        /// <param name="blockedID">Korisnik koji se blokira</param>
        /// <response code="204">Uspesno obrisan tip ocene.</response>
        /// <response code="401"> Neuspesna autorizacija korisnika.</response>
        /// <response code="404">Korisnik pokusava da obrise nepostojeci tip ocene.</response>
        /// <response code="500">Greska na serveru.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("blockerID/{blockerID}/blockedID/{blockedID}")]
        public IActionResult Unblock([FromHeader] string key, int blockerID, int blockedID)
        {
            if (!_authService.Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User authorization failed!");
            }

            try
            {
                _blockingService.Unblock(blockerID, blockedID);

                return NoContent();
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error while unblocking user: " + ex.Message);
                

                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }


        /// <summary>
        /// Prikaz HTTP metoda koje korisnik moze da pozove.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za prikaz dostupnih HTTP metoda
        /// OPTIONS 'http://localhost:4207/api/block' \
        /// </remarks>
        /// <response code="200">Uspesno prikazane dostupne metode.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetBlockOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
