using AutoMapper;
using MessagingService.Attributes;
using MessagingService.Dtos;
using MessagingService.Entities;
using MessagingService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Controllers
{
    [Route("api/conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ConversationsController(
            IConversationRepository conversationRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new Conversation.
        /// </summary>
        /// <param name="request">Dto that contains values needed for creating Conversation.</param>
        /// <response code="201">Creates new Conversation.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>New Conversation.</returns>
        [HttpPost]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public ActionResult<ConversationReadDto> Create([FromBody] ConversationCreateDto request)
        {
            if (_userRepository.Get(request.CreatorId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            if (request.CreatorId != request.ParticipantId && _userRepository.Get(request.ParticipantId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            Conversation newEntity = _mapper.Map<Conversation>(request);

            newEntity = _conversationRepository.Create(newEntity);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<ConversationReadDto>(newEntity));
        }

        /// <summary>
        /// Returns all Conversations.
        /// </summary>
        /// <response code="200">Conversations returned.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All Conversations.</returns>
        [HttpGet]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ConversationReadDto>> Get()
        {
            List<Conversation> result = _conversationRepository.Get().ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ConversationReadDto>>(result));
        }

        /// <summary>
        /// Returns all Conversations for User.
        /// </summary>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="200">Conversations returned.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All User's Conversations.</returns>
        [HttpGet("for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ConversationReadDto>> GetForUser(int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            List<Conversation> result = _conversationRepository.Get().Where(e => e.CreatorId == userId || e.ParticipantId == userId).ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ConversationReadDto>>(result));
        }

        /// <summary>
        /// Returns Conversation with provided id.
        /// </summary>
        /// <param name="id">Id of requested Conversation.</param>
        /// <response code="200">Conversation returned.</response>
        /// <response code="400">Non-existing Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>Conversation by id.</returns>
        [HttpGet("{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ConversationReadDto> Get(int id)
        {
            Conversation result = _conversationRepository.Get(id);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ConversationReadDto>(result));
        }

        /// <summary>
        /// Returns Conversation with provided id for User.
        /// </summary>
        /// <param name="id">Id of requested Conversation.</param>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="200">Conversation returned.</response>
        /// <response code="400">Non-existing User or Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of Conversation.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>Conversation by id for User.</returns>
        [HttpGet("{id}/for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ConversationReadDto> GetForUser(int id, int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            Conversation result = _conversationRepository.Get(id);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            if (result.CreatorId != userId && result.ParticipantId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You can't access Conversations you're not a part of.");
            }

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ConversationReadDto>(result));
        }

        /// <summary>
        /// Deletes Conversation.
        /// </summary>
        /// <param name="id">Id of requested Conversation.</param>
        /// <response code="204">Deletes Conversation with provided id.</response>
        /// <response code="400">Non-existing Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(int id)
        {
            _conversationRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes Conversations for User.
        /// </summary>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="204">Deletes Conversations that belong to User with provided id.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns></returns>
        [HttpDelete("for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteAllForUser(int id, int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            List<Conversation> found = _conversationRepository.Get().Where(e => e.CreatorId == userId || e.ParticipantId == userId).ToList();

            foreach (Conversation entity in found)
            {
                _conversationRepository.Delete(id);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes Conversation for User.
        /// </summary>
        /// <param name="id">Id of requested Conversation.</param>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="204">Deletes Conversation with provided Id and checks if it belongs to User with provided id.</response>
        /// <response code="400">Non-existing User or Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of Conversation.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns></returns>
        [HttpDelete("{id}/for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteForUser(int id, int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            Conversation found = _conversationRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            if (found.CreatorId != userId && found.ParticipantId != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You can't access Conversations you're not a part of.");
            }

            _conversationRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
