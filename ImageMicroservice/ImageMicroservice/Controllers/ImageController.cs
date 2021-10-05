using ImageMicroservice.Dto;
using ImageMicroservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _service;

        public ImageController(IImageService service)
        {
            _service = service;
        }



        /// <summary>
        /// Vraca slike za odredjeni post.
        /// </summary>
        /// <param name="postID">ID posta</param>
        /// <returns>Fajl sa slikama posta.</returns>
        /// <remarks>
        /// 
        /// POST 'http://localhost:44315/post/postId'
        /// 
        /// </remarks>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slike su uspesno vracene</response>
        [HttpGet("post/{postID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetImagesForPost(long postID)
        {
            var (fileType, archiveData, archiveName) = _service.GetAllImagesForPost(postID);

            return File(archiveData, fileType, archiveName);
        }


        /// <summary>
        /// Vraca slike za odredjenog korisnika.
        /// </summary>
        /// <param name="userID">ID korisnika.</param>
        /// <returns>Fajl sa slikama korisnika.</returns>
        /// <remarks>
        /// GET http://localhost:44315/user/userId'
        /// </remarks>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slike su uspesno vracene</response>
        [HttpGet("user/{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetImagesForUser(long userID)
        {
            var (fileType, archiveData, archiveName) = _service.GetAllImagesForUser(userID);

            return File(archiveData, fileType, archiveName);
        }




        /// <summary>
        /// Vraca sliku po id-ju.
        /// </summary>
        /// <param name="id">ID slike.</param>
        /// <returns>Fajl sa slikom.</returns>7
        /// <remarks>
        /// GET  http://localhost:44315/id
        /// </remarks>
        /// <response code="404">Slika nije pronadjena.</response>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slika je uspesno vracena.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByID(long id)
        {

            ImageDownloadDto dto = _service.GetImageByImageID(id);
            return File(dto.File, "application/octet-stream", dto.ImagePath);
        }

        /// <summary>
        /// Kreira novu sliku.
        /// </summary>
        /// <param name="image">Slika koja treba da se kreira.</param>
        /// <returns>Potvrdu o kreiranju.</returns>
        /// <remarks>
        /// POST http://localhost:44315/
        /// </remarks>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slika je uspesno kreirana.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(ImageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateImage([FromForm] ImageUploadDto image)
        {
            return Ok(_service.CreateImage(image));
        }

        /// <summary>
        /// Brise sliku
        /// </summary>
        /// <param name="id">Id slike.</param>
        /// <returns>Potvrdu o brisanju.</returns>
        /// DELETE http://localhost:44315/id
        /// <response code="404">Slika nije pronadjena.</response>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slika je uspesno obrisana.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteByid (long id)
        {
            _service.DeleteImage(id);
            return Ok();
        }

        /// <summary>
        /// Brise slike za post.
        /// </summary>
        /// <param name="id">Id posta</param>
        /// <remarks>
        /// DELETE http://localhost:44315/post/id
        /// </remarks>
        /// <returns>Potvrdu o brisanju.</returns>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slika je uspesno obrisane.</response>
        [HttpDelete("post/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteImageByPost(long id)
        {
            _service.DeleteImagesByPostID(id);
            return Ok();
        }


        /// <summary>
        /// Brise slike po korisniku.
        /// </summary>
        /// <param name="id">Id korisnika</param>
        /// <returns>Potvrdu o brisanju.</returns>
        /// <remarks>
        /// DELETE http://localhost:44315/user/id
        /// </remarks>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="200">Slika je uspesno obrisane.</response>
        [HttpDelete("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteImageByUser(long id)
        {
            _service.DeleteImagesByUserID(id);
            return Ok();
        }

    }
}
