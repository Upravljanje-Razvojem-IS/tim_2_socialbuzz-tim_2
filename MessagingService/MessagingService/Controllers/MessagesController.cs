using AutoMapper;
using MessagingService.Attributes;
using MessagingService.Dtos;
using MessagingService.Entities;
using MessagingService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(
            IMessageRepository messageRepository,
            IConversationRepository conversationRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new Message.
        /// </summary>
        /// <param name="request">Dto that contains values needed for creating Message.</param>
        /// <response code="201">Creates new Message.</response>
        /// <response code="400">Non-existing User or Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of Conversation for which the Message should be created.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>New Message.</returns>
        [HttpPost]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public ActionResult<MessageReadDto> Create([FromBody] MessageCreateDto request)
        {
            if (_userRepository.Get(request.SenderId) == null || _userRepository.Get(request.ReceiverId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            Conversation conversation = _conversationRepository.Get(request.ConversationId);

            if (conversation == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            if (conversation.CreatorId != conversation.ParticipantId && request.SenderId == request.ReceiverId)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "You can't send message to yourself in Conversation with someone else.");
            }

            if ((conversation.CreatorId != request.SenderId &&
                conversation.CreatorId != request.ReceiverId) ||
                (conversation.ParticipantId != request.SenderId &&
                conversation.ParticipantId != request.ReceiverId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Cannot create Message for Conversation you're not a part of.");
            }

            Message newEntity = _mapper.Map<Message>(request);

            newEntity.DateTime = DateTime.UtcNow;
            newEntity.StatusRead = false;

            newEntity = _messageRepository.Create(newEntity);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<MessageReadDto>(newEntity));
        }

        /// <summary>
        /// Returns all Messages.
        /// </summary>
        /// <response code="200">Messages returned.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All Messages.</returns>
        [HttpGet]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<MessageReadDto>> Get()
        {
            List<Message> result = _messageRepository.Get().ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<MessageReadDto>>(result));
        }

        /// <summary>
        /// Returns all Messages for Conversation.
        /// </summary>
        /// <param name="conversationId">Id of requested Conversation.</param>
        /// <response code="200">Messages returned.</response>
        /// <response code="400">Non-existing Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All Messages.</returns>
        [HttpGet("for-conversation/{conversationId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<MessageReadDto>> GetForConversation(int conversationId)
        {
            if (_conversationRepository.Get(conversationId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            List<Message> result = _messageRepository.Get().Where(e => e.ConversationId == conversationId).ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<MessageReadDto>>(result));
        }

        /// <summary>
        /// Returns Message with provided id.
        /// </summary>
        /// <param name="id">Id of requested Message.</param>
        /// <response code="200">Message returned.</response>
        /// <response code="400">Non-existing Message.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>Message by id.</returns>
        [HttpGet("{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MessageReadDto> Get(int id)
        {
            Message result = _messageRepository.Get(id);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Message doesn't exist.");
            }

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<MessageReadDto>(result));
        }

        /// <summary>
        /// Updates Messages read statuses.
        /// </summary>
        /// <param name="id">Id of requested Message.</param>
        /// <param name="conversationId">Id of requested Conversation.</param>
        /// <response code="200">Message updated and returned.</response>
        /// <response code="400">Non-existing Conversation or Message, or Message doesn't belong to Conversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns></returns>
        [HttpPut("read/{conversationId}/{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateAsReadForConversation(int conversationId, int id)
        {
            if (_conversationRepository.Get(conversationId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Conversation doesn't exist.");
            }

            Message found = _messageRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Message doesn't exist.");
            }

            if (found.ConversationId != conversationId)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Cannot update Message that doesn't belong to Conversation with provided id.");
            }

            List<Message> messages = _messageRepository.Get()
                .Where(e =>
                    e.ConversationId == conversationId &&
                    e.Id < id && 
                    !e.StatusRead
                ).ToList();

            messages.Add(found);

            foreach (Message message in messages)
            {
                message.StatusRead = true;

                _messageRepository.Update(message);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes Message.
        /// </summary>
        /// <param name="id">Id of requested Message.</param>
        /// <response code="204">Deletes Message with provided id.</response>
        /// <response code="400">Non-existing Message.</response>
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
            _messageRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
