using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ReactionService.Attributes;
using ReactionService.Dtos;
using ReactionService.Entities;
using ReactionService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactionService.Controllers
{
    [Route("api/reactions")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IReactionTypeRepository _reactionTypeRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ReactionsController(
            IReactionRepository reactionRepository,
            IReactionTypeRepository reactionTypeRepository,
            IPostRepository postRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _reactionRepository = reactionRepository;
            _reactionTypeRepository = reactionTypeRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new Reaction.
        /// </summary>
        /// <param name="request">Dto containing values required for creating Reaction.</param>
        /// <returns>New Reaction.</returns>
        /// <response code="201">Creates new Reaction.</response>
        /// <response code="400">Invalid value in body or non-existing User, Post or ReactionType.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Authorization]
        [HttpPost]
        public ActionResult<ReactionReadDto> Create([FromBody] ReactionCreateDto request)
        {
            if (_userRepository.Get(request.UserId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            if (_postRepository.Get(request.PostId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Post doesn't exist.");
            }

            if (_reactionTypeRepository.Get(request.ReactionTypeId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction type doesn't exist.");
            }

            Reaction newEntity = _mapper.Map<Reaction>(request);

            newEntity.DateTime = DateTime.UtcNow;

            Reaction result = _reactionRepository.Create(newEntity);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<ReactionReadDto>(result));
        }

        /// <summary>
        /// Returns all Reactions for Post.
        /// </summary>
        /// <param name="postId">Id of Post whose Reactions will be returned.</param>
        /// <returns>Reactions for Post.</returns>
        /// <response code="200">Reactions returned.</response>
        /// <response code="400">Non-existing Post.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("for-post/{postId}")]
        public ActionResult<IEnumerable<ReactionReadDto>> GetForPost(int postId)
        {
            if (_postRepository.Get(postId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Post doesn't exist.");
            }

            List<Reaction> result = _reactionRepository.Get().Where(e => e.PostId == postId).ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ReactionReadDto>>(result));
        }

        /// <summary>
        /// Returns all Reactions for Post by ReactionType.
        /// </summary>
        /// <param name="postId">Id of Post whose Reactions will be returned.</param>
        /// <param name="reactionTypeId">Id of ReactionType by which Reactions will be filtered.</param>
        /// <returns>Reactions for Post by ReactionType.</returns>
        /// <response code="200">Reactions returned.</response>
        /// <response code="400">Non-existing Post or ReactionType.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("for-post/{postId}/by-reaction-type/{reactionTypeId}")]
        public ActionResult<IEnumerable<ReactionReadDto>> GetForPostByReactionType(int postId, int reactionTypeId)
        {
            if (_postRepository.Get(postId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Post doesn't exist.");
            }

            if (_reactionTypeRepository.Get(reactionTypeId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction type doesn't exist.");
            }

            List<Reaction> result = _reactionRepository.Get().Where(e => e.PostId == postId && e.ReactionTypeId == reactionTypeId).ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ReactionReadDto>>(result));
        }

        /// <summary>
        /// Return Reaction with provided id.
        /// </summary>
        /// <param name="id">Id of requested Reaction.</param>
        /// <returns>Reaction by id.</returns>
        /// <response code="200">Reaction returned.</response>
        /// <response code="400">Non-existing Reaction.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<ReactionReadDto> Get(int id)
        {
            if (_reactionRepository.Get(id) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction doesn't exist.");
            }

            Reaction result = _reactionRepository.Get(id);

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ReactionReadDto>(result));
        }

        /// <summary>
        /// Updates Reaction.
        /// </summary>
        /// <param name="id">Id of Reaction that needs to be updated.</param>
        /// <param name="request">Dto containing updated values.</param>
        /// <returns>Updated Reaction.</returns>
        /// <response code="200">Reaction updated.</response>
        /// <response code="400">Invalid value in body or non-existing ReactionType or Reaction.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Authorization]
        [HttpPut("{id}")]
        public ActionResult<ReactionReadDto> Update(int id, [FromBody] ReactionUpdateDto request)
        {
            if (_reactionTypeRepository.Get(request.ReactionTypeId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction type doesn't exist.");
            }

            Reaction found = _reactionRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction doesn't exist.");
            }

            found = _mapper.Map(request, found);

            _reactionRepository.Update(found);

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ReactionReadDto>(found));
        }

        /// <summary>
        /// Deletes Reaction.
        /// </summary>
        /// <param name="id">Id of requested Reaction.</param>
        /// <returns></returns>
        /// <response code="204">Deletes Reaction with provided id.</response>
        /// <response code="400">Non-existing Reaction.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorization]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (_reactionRepository.Get(id) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Reaction doesn't exist.");
            }

            _reactionRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes all User's Reactions.
        /// </summary>
        /// <param name="userId">Id of User whose Reactions need to be deleted.</param>
        /// <returns></returns>
        /// <response code="204">Deletes Reactions with provided userId.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorization]
        [HttpDelete("for-user/{userId}")]
        public ActionResult DeleteForUser(int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            List<Reaction> found = _reactionRepository.Get().Where(e => e.UserId == userId).ToList();

            foreach (Reaction entity in found)
            {
                _reactionRepository.Delete(entity.Id);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes Reactions for all User's Posts.
        /// </summary>
        /// <param name="userId">Id of User whose Post's Reactions need to be deleted.</param>
        /// <returns></returns>
        /// <response code="204">Deletes Reaction with postId from User's Posts.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorization]
        [HttpDelete("on-posts-by-user/{userId}")]
        public ActionResult DeleteOnPostsByUser(int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            List<Post> usersPosts = _postRepository.Get().Where(e => e.UserId == userId).ToList();

            if (usersPosts.Count() == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            List<Reaction> found = _reactionRepository.Get().Where(e => usersPosts.Any(up => up.Id == e.PostId)).ToList();

            foreach (Reaction entity in found)
            {
                _reactionRepository.Delete(entity.Id);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes all Reactions for Post.
        /// </summary>
        /// <param name="postId">Id of Post whose Reactions need to be deleted.</param>
        /// <returns></returns>
        /// <response code="204">Deletes Reactions with provided postId.</response>
        /// <response code="400">Non-existing Post.</response>
        /// <response code="401">User is not authorized</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorization]
        [HttpDelete("for-post/{postId}")]
        public ActionResult DeleteForPost(int postId)
        {
            if (_postRepository.Get(postId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Post doesn't exist.");
            }

            List<Reaction> found = _reactionRepository.Get().Where(e => e.PostId == postId).ToList();

            foreach (Reaction entity in found)
            {
                _reactionRepository.Delete(entity.Id);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
