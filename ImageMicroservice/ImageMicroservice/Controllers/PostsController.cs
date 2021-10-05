using ImageMicroservice.Dto;
using ImageMicroservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        /// <summary>
        /// Vraca postove sa zida.
        /// </summary>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// GET http://localhost:44315/
        /// </remarks>
        /// <response code="200">Post je uspesno vracen.</response>
        /// <response code="204">Ne postoje postovi.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllFromWall()
        {
            return Ok(_postService.GetAllPostsFromWall());
        }

        /// <summary>
        /// Vraca postove po ID-ju.
        /// </summary>
        /// <param name="id">Id posta.</param>
        /// <returns>Listu postova</returns>
        /// <remarks>
        /// GET http://localhost:44315/id
        /// </remarks>
        /// <response code="200">Post je uspesno vracen.</response>
        /// <response code="204">No content.</response>
        /// <response code="400">Neispravan zahtev.</response>
        /// <response code="404">Ne postoji id posta.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(long id)
        {
            return Ok(_postService.GetPostById(id));
        }

        /// <summary>
        /// Vraca postove po naslovu.
        /// </summary>
        /// <param name="title">Naslov posta.</param>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// GET http://localhost:44315/title/title
        /// </remarks>
        /// <response code="200">Post je uspesno vracen.</response>
        /// <response code="204">No content.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpGet("title/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetByTitle(string title)
        {
            return Ok(_postService.GetPostsByTitle(title));
        }

        /// <summary>
        /// Vraca postove po korisniku.
        /// </summary>
        /// <param name="userID">Id korisnika</param>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// GET http://localhost:44315/user/userID
        /// </remarks>
        /// <response code="200">Post je uspesno vracen.</response>
        /// <response code="204">No content.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpGet("user/{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetByUserID(long userID)
        {
            return Ok(_postService.GetPostsByUserID(userID));
        }

        /// <summary>
        /// Vraca postove po gradu.
        /// </summary>
        /// <param name="city">Grad posta.</param>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// GET http://localhost:44315/city/city
        /// </remarks>
        /// <response code="200">Post je uspesno vracen.</response>
        /// <response code="204">No content.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpGet("city/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetByCity(string city)
        {
            return Ok(_postService.GetPostsByCity(city));
        }

        /// <summary>
        /// Dodaje novi post.
        /// </summary>
        /// <param name="post">Post koji je poterbno dodati.</param>
        /// <returns>Potvrdu o kreiranju.</returns>
        /// <remarks>
        /// POST http://localhost:44315/
        /// </remarks>
        /// <response code="201">Post je kreiran.</response>
        /// <response code="400">Neispravan zahtev.</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePost([FromBody] PostDto post)
        {
            PostDto newPost = _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetById), new { id = newPost.PostID }, newPost);
        }

        /// <summary>
        /// Azurira post.
        /// </summary>
        /// <param name="id">Id posta</param>
        /// <param name="post">Post</param>
        /// <returns>Potvrdu o kreiranju posta.</returns>
        /// <remarks>
        /// PUT http://localhost:44315/id
        /// </remarks>
        /// <response code="200">Post je uspesno azuriran.</response>
        /// <response code="404">Post nije pronadjen.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePost(long id, [FromBody] PostDto post )
        {
            PostDto newPost = _postService.UpdatePost(id, post);
            return Ok(newPost);
        }

        /// <summary>
        /// Brise post.
        /// </summary>
        /// <param name="id">Id posta</param>
        /// <returns>Potvrdu o brisanju.</returns>
        /// <remarks>
        /// DELETE http://localhost:44315/id
        /// </remarks>
        /// <response code="204">Post je uspesno obrisan.</response>
        /// <response code="404">Post nije pronadjen.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePost(long id)
        {
            _postService.DeletePost(id);
            return Ok();
        }

        /// <summary>
        /// Brisanje postova po korisniku
        /// </summary>
        /// <param name="id">Id korisnika</param>
        /// <returns>Potvrdu o brisanju</returns>
        /// <remarks>
        /// DELETE http://localhost:44315/user/id
        /// </remarks>
        /// <response code="204">Post je uspesno obrisan.</response>
        /// <response code="400">Neispravan zahtev.</response>
        [HttpDelete("user/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeletePostsByUserID(long id)
        {
            _postService.DeletePostsByUserID(id);
            return Ok();
        }
    }
}
