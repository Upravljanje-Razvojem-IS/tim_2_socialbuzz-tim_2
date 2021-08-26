using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PostService.AuthorizationMock;
using PostService.Logger;
using PostService.Models;
using PostService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api")]
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IMapper autoMapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IAuthorizationMockService authorizationMockService;
        private readonly ILoggerRepository<PostController> loggerRepository;
        public PostController(IPostService postService, IMapper autoMapper, LinkGenerator linkGenerator, IAuthorizationMockService authorizationMockService, ILoggerRepository<PostController> loggerRepository)
        {
            this.postService = postService;
            this.autoMapper = autoMapper;
            this.linkGenerator = linkGenerator;
            this.authorizationMockService = authorizationMockService;
            this.loggerRepository = loggerRepository;
        }



        /// <summary>
        /// Kreira novi post.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="newPost">Novi post tipa PostCreationDto.</param>
        /// <returns>Potvrdu o kreiranju posta.</returns>
        /// <remarks>
        /// Primer zahteva za kreiranje novog posta.
        /// {
        /// "postTitle" : "Asus VivoBook",
        /// "postDescription" : "Asus VivoBook Max nov, jednom koriscen za majnovanje bitkoina ali nista strasno. Radi kao sat."
        /// "City" : "Novi Sad",
        /// "UserId" : 2
        /// }
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("post")]
        public ActionResult<PostDto> CreateNewPost([FromHeader] string secretToken, [FromBody] PostCreationDto newPost)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var toBeCreated = postService.CreatePost(newPost);
                    loggerRepository.LogInformation("New post successfuly created!");
                    return StatusCode(201, toBeCreated);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Briše post iz baze.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="PostId">Id posta koji želimo da obrišemo.</param>
        /// <returns>Potvrdu o brisanju.</returns>
        /// <remarks>
        /// Primer zahteva za brisanje posta.
        /// Id posta se čita iz query parametra.
        /// DELETE localhost:44200/api/c4cb6501-d98d-4297-2fab-08d9665f8d17?PostId=51db2a5e-5b2c-4724-1e99-08d96662d1cd
        /// </remarks>
        /// <response code="204">Post je uspešno obrisan.</response>
        /// <response code="401">Neuspešna autorizacija.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{postId}")]
        public IActionResult DeletePost([FromHeader] string secretToken, [FromQuery] Guid PostId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    postService.DeletePost(PostId);
                    loggerRepository.LogInformation("Post deleted successfully!");
                    return StatusCode(StatusCodes.Status204NoContent, "Post successfully deleted!");
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Post not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "An error occured while deleting post : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while deleting post : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća post po Id-ju posta.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="PostId">Id posta koji želimo da vratimo.</param>
        /// <returns>PostDto reprezentaciju posta.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje posta po Id-ju.
        /// Id posta se čita iz query parametra.
        /// GET localhost:44200/api/post/?PostId=a39e1850-1ca6-4805-270b-08d96663b948
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("post")]
        public ActionResult<PostDto> GetPostById([FromHeader] string secretToken, [FromQuery] Guid PostId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPost = postService.GetPostById(PostId);
                    loggerRepository.LogInformation("Post successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPost);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "Post not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća postove po Id-ju korisnika.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="UserId">Id korisnika čije postove želimo da vidimo.</param>
        /// <returns>Listu postova određenog korisnika.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje posta po Id-ju korisnika.
        /// Id se čita iz query parametra.
        /// GET localhost:44200/api/postsByUserId/?UserId=2
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("postsByUserId")]
        public ActionResult<List<PostDto>> GetPostsByUserId([FromHeader] string secretToken, [FromQuery] int UserId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByUserId(UserId);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    } else if (ex.Message == "User blocked!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Vraća sve postove koji se trenutno nalaze u bazi. Ne uzima u obzir bilo kakve parametre
        /// osim tokena za autorizaciju. Token je tu pod pretpostavkom da ovoj ruti može da
        /// pristupi samo ulogovani korisnik.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <returns>Listu objekata tipa PostDto, odnosno listu svih postova.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje svih postova.
        /// GET localhost:44200/api/posts
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("posts")]
        public ActionResult<List<PostDto>> GetAllPosts([FromHeader] string secretToken)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPosts();
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }



        /// <summary>
        /// Vraća postove po gradovima.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="City">Grad po kojem vršimo filtriranje.</param>
        /// <returns>Listu postova u određenom gradu.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje postova po gradu.
        /// Grad se čita iz query parametra.
        /// GET localhost:44200/api/postsByCity?City=Beograd
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("postsByCity")]
        public ActionResult<List<PostDto>> GetPostsByCity([FromHeader] string secretToken, [FromQuery] string City)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByCity(City);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "No posts found in city : " + City)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Vraća postove po naslovu.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="Title">Naslov posta.</param>
        /// <returns>Listu postova sa zajedničkim naslovom.</returns>
        /// <remarks>
        /// Primer zahteva za vraćanje postova po naslovu.
        /// Naslov se čita iz query parametra.
        /// GET localhost:44200/api/postsByTitle?Title=Asus Vivobook
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("postsByTitle")]
        public ActionResult<List<PostDto>> GetPostsByTitle([FromHeader] string secretToken, [FromQuery] string Title)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsByTitle(Title);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "No posts found with title : " + Title)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Briše postove po Id-ju korisnika.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju. U realnoj situaciji JWT Token generisan na osnovu 
        /// korisničkih podataka.</param>
        /// <param name="UserId">Id korisnika čije postove želimo da obrišemo.</param>
        /// <returns>Potvrdu o uspešnom brisanju postova.</returns>
        /// <remarks>
        /// Ovaj zahtev služi u slučaju da želimo da obrišemo korisnika. Ako ne postoji korisnik u bazi ne smeju da postoje
        /// postovi koje je taj korisnik namestio. 
        /// 
        /// Primer zahteva za brisanje postova po Id-ju.
        /// Id korisnika se čita iz query parametra.
        /// DELETE localhost:44200/api/deleteByUserId?UserId=2
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("deleteByUserId")]
        public IActionResult DeletePostsByUserId([FromHeader] string secretToken, [FromQuery] int UserId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    postService.DeletePostsByUserId(UserId);
                    loggerRepository.LogInformation("Post successfully deleted!");
                    return StatusCode(StatusCodes.Status204NoContent, "Posts deleted successfully!");
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error deleting posts : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }


        /// <summary>
        /// Vraća postove za korisnika kojeg pratimo.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="UserId">Id korisnika koji šalje zahtev.</param>
        /// <param name="SubjectId">Id korisnika čije postove želimo da vratimo.</param>
        /// <returns>Listu postova.</returns>
        /// <remarks>
        /// Da bi ovaj zahtev bio uspešan prvo moramo da pratimo korisnika čije postove želimo da vratimo. 
        /// User Id i Subject Id nam služe za proveru praćenja i blokiranja pre vraćanja podataka.
        /// 
        /// Primer zahteva za vraćanje postova.
        /// 
        /// UserId i SubjectId se čitaju iz query parametara.
        /// GET localhost:44200/api/getPostsFromWall?UserId=1&SubjectId=2
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("getPostsFromWall")]
        public IActionResult GetPostsFromWall([FromHeader] string secretToken, [FromQuery] int UserId, [FromQuery] int SubjectId)
        {
            if (authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var fetchedPosts = postService.GetPostsFromWall(UserId, SubjectId);
                    loggerRepository.LogInformation("Posts successfully fetched!");
                    return StatusCode(StatusCodes.Status200OK, fetchedPosts);
                }
                catch (Exception ex)
                {
                    if(ex.Message == "User not found!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error fetching posts : " + ex.Message);
                    } else if (ex.Message == "User blocked!" || ex.Message == "You are not following that user!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Error fetching posts : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }

        /// <summary>
        /// Ažurira post.
        /// </summary>
        /// <param name="secretToken">Token za autorizaciju.</param>
        /// <param name="post">Nova verzija posta.</param>
        /// <returns>Potvrdu o ažuriranju posta.</returns>
        /// <remarks>
        /// Primer zahteva za ažuriranje posta.
        /// 
        /// PUT localhost:44200/api/c814e683-5d07-4d35-16cd-08d967e4050a
        /// {
        /// "postId": "c814e683-5d07-4d35-16cd-08d967e4050a",
        /// "postDescription": "Laptop",
        /// "postTitle": "Asus",
        /// "city": "Novi Sad"
        ///}
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [HttpPut("{PostId}")]
        public IActionResult UpdatePost([FromHeader] string secretToken, [FromBody] PostModificationDto post)
        {
            if(authorizationMockService.AuthorizeToken(secretToken))
            {
                try
                {
                    var updatedPost = postService.UpdatePost(post);
                    loggerRepository.LogInformation("Post successfully updated!");
                    return StatusCode(StatusCodes.Status200OK, updatedPost);

                } catch (Exception ex)
                {
                    if(ex.Message == "Post with that Id does not exist!")
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Error updating posts : " + ex.Message);
                    } else if (ex.Message == "Changing of the post Id is not permitted!")
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Error updating posts : " + ex.Message);
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating posts : " + ex.Message);
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "Authorization failed!");
        }
    }
}
