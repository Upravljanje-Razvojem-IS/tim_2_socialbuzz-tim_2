using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostService.Data.TypeOfPostRepository;
using PostService.Entities.Type;
using PostService.Models;
using PostService.Services.TypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/typeOfPost")]
    public class TypeOfPostController : ControllerBase
    {
        private readonly ITypeOfPostRepository typeRepository;
        private readonly ITypeOfPostService typeService;
        private readonly IMapper autoMapper;

        public TypeOfPostController(ITypeOfPostRepository typeRepository, IMapper autoMapper, ITypeOfPostService typeService)
        {
            this.typeRepository = typeRepository;
            this.autoMapper = autoMapper;
            this.typeService = typeService;
        }

        [HttpPost("type")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TypeOfPostCreationConfirmationDto> CreateType([FromBody] TypeOfPostCreationDto type)
        {
            try
            {
                var typeOfPost = autoMapper.Map<TypeOfPost>(type);
                TypeOfPostCreatedConfirmation createdType = typeService.CreateType(typeOfPost);
                return StatusCode(201, createdType);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{TypeId}")]
        public IActionResult DeleteType(Guid TypeId)
        {
            try
            {
                typeService.DeleteType(TypeId);
                return NoContent();
            } catch (Exception ex)
            {
                if(ex.Message == "Type not found!")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                } else if(ex.Message == "Foreign key constraint violated!")
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{TypeId}")]
        public ActionResult<TypeOfPostDto> GetTypeById(Guid TypeId)
        {
            try
            {
                var type = typeService.GetTypeById(TypeId);
                return StatusCode(StatusCodes.Status200OK, type);
            } catch (Exception ex)
            {
                if(ex.Message == "Type not found!")
                {
                    return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
