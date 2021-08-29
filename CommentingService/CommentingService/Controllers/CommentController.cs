using CommentingService.DTOs;
using CommentingService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CommentingService.Controllers
{
    [ApiController]
    [Route("api/comment")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repository;

        public CommentController(ICommentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all comments
        /// </summary>
        /// <returns>List of comments</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _repository.Get();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>comment</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>New comment</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public ActionResult PostComment([FromBody] CommentCreateDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>Updated comment</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}")]
        public ActionResult PutComment(Guid id, CommentCreateDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public ActionResult DeleteComment(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
