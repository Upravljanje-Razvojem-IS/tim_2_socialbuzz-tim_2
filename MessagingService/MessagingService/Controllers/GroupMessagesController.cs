using AutoMapper;
using MessagingService.Attributes;
using MessagingService.Dtos;
using MessagingService.Entities;
using MessagingService.Helper;
using MessagingService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Controllers
{
    [Route("api/group-messages")]
    [ApiController]
    public class GroupMessagesController : ControllerBase
    {
        private readonly IGroupMessageRepository _groupMessageRepository;
        private readonly IGroupConversationRepository _groupConversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GroupMessagesController(
            IGroupMessageRepository groupMessageRepository,
            IGroupConversationRepository groupConversationRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _groupMessageRepository = groupMessageRepository;
            _groupConversationRepository = groupConversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new GroupMessage.
        /// </summary>
        /// <param name="request">Dto that contains values needed for creating GroupMessage.</param>
        /// <response code="201">Creates new GroupMessage.</response>
        /// <response code="400">Non-existing User or GroupConversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of GroupConversation for which the GroupMessage should be created.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>New GroupMessage.</returns>
        [HttpPost]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public ActionResult<GroupMessageReadDto> Create([FromBody] GroupMessageCreateDto request)
        {
            if (_userRepository.Get(request.SenderId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            GroupConversation groupConversation = _groupConversationRepository.Get(request.GroupConversationId);

            if (groupConversation == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "GroupConversation doesn't exist.");
            }

            if (groupConversation.CreatorId != request.SenderId && !FormatHelper.CsvToListInt(groupConversation.ParticipantIds).Any(e => e == request.SenderId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Cannot create GroupMessage for GroupConversation you're not a part of.");
            }

            GroupMessage newEntity = _mapper.Map<GroupMessage>(request);

            newEntity.DateTime = DateTime.UtcNow;

            newEntity = _groupMessageRepository.Create(newEntity);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<GroupMessageReadDto>(newEntity));
        }

        /// <summary>
        /// Returns all GroupMessages.
        /// </summary>
        /// <response code="200">GroupMessages returned.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All GroupMessages.</returns>
        [HttpGet]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<GroupMessageReadDto>> Get()
        {
            List<GroupMessage> result = _groupMessageRepository.Get().ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<GroupMessageReadDto>>(result));
        }

        /// <summary>
        /// Returns all GroupMessages for GroupConversation.
        /// </summary>
        /// <param name="groupConversationId">Id of requested GroupConversation.</param>
        /// <response code="200">GroupMessages returned.</response>
        /// <response code="400">Non-existing GroupConversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All GroupMessages.</returns>
        [HttpGet("for-group-conversation/{groupConversationId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<GroupMessageReadDto>> GetForGroupConversation(int groupConversationId)
        {
            if (_groupConversationRepository.Get(groupConversationId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "GroupConversation doesn't exist.");
            }

            List<GroupMessage> result = _groupMessageRepository.Get().Where(e => e.GroupConversationId == groupConversationId).ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<GroupMessageReadDto>>(result));
        }

        /// <summary>
        /// Returns GroupMessage with provided id.
        /// </summary>
        /// <param name="id">Id of requested GroupMessage.</param>
        /// <response code="200">GroupMessage returned.</response>
        /// <response code="400">Non-existing GroupMessage.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>GroupMessage by id.</returns>
        [HttpGet("{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<GroupMessageReadDto> Get(int id)
        {
            GroupMessage result = _groupMessageRepository.Get(id);

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<GroupMessageReadDto>(result));
        }

        /// <summary>
        /// Deletes GroupMessage.
        /// </summary>
        /// <param name="id">Id of requested GroupMessage.</param>
        /// <response code="204">Deletes GroupMessage with provided id.</response>
        /// <response code="400">Non-existing GroupMessage.</response>
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
            _groupMessageRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
