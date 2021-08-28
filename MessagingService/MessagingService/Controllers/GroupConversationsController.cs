using AutoMapper;
using MessagingService.Attributes;
using MessagingService.Dtos;
using MessagingService.Entities;
using MessagingService.Helper;
using MessagingService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MessagingService.Controllers
{
    [Route("api/group-conversations")]
    [ApiController]
    public class GroupConversationsController : ControllerBase
    {
        private readonly IGroupConversationRepository _groupConversationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GroupConversationsController(
            IGroupConversationRepository groupconversationRepository,
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _groupConversationRepository = groupconversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new GroupConversation.
        /// </summary>
        /// <param name="request">Dto that contains values needed for creating GroupConversation.</param>
        /// <response code="201">Creates new GroupConversation.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>New GroupConversation.</returns>
        [HttpPost]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        public ActionResult<GroupConversationReadDto> Create([FromBody] GroupConversationCreateDto request)
        {
            if (_userRepository.Get(request.CreatorId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            if (request.ParticipantIdsList.Any(e => _userRepository.Get(e) == null))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "One or more Users don't exist.");
            }

            GroupConversation newEntity = _mapper.Map<GroupConversation>(request);

            newEntity.ParticipantIds = FormatHelper.ListIntToCsv(request.ParticipantIdsList);

            newEntity = _groupConversationRepository.Create(newEntity);

            GroupConversationReadDto result = _mapper.Map<GroupConversationReadDto>(newEntity);

            result.ParticipantIdsList = request.ParticipantIdsList;

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Returns all GroupConversations.
        /// </summary>
        /// <response code="200">GroupConversations returned.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All GroupConversations.</returns>
        [HttpGet]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<GroupConversationReadDto>> Get()
        {
            List<GroupConversation> found = _groupConversationRepository.Get().ToList();

            List<GroupConversationReadDto> result = new List<GroupConversationReadDto>();

            foreach (GroupConversation item in found)
            {
                GroupConversationReadDto dto = _mapper.Map<GroupConversationReadDto>(item);

                dto.ParticipantIdsList = FormatHelper.CsvToListInt(item.ParticipantIds);

                result.Add(dto);
            }

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<GroupConversationReadDto>>(result));
        }

        /// <summary>
        /// Returns all GroupConversations for User.
        /// </summary>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="200">GroupConversations returned.</response>
        /// <response code="400">Non-existing User.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>All User's GroupConversations.</returns>
        [HttpGet("for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<GroupConversationReadDto>> GetForUser(int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            Dictionary<int, List<int>> formattedParticipantIdsDictionary = new Dictionary<int, List<int>>();

            List<GroupConversation> found = _groupConversationRepository.Get()
                .Where(e =>
                {
                    List<int> formattedParticipantIds = FormatHelper.CsvToListInt(e.ParticipantIds);

                    formattedParticipantIdsDictionary.Add(e.Id, formattedParticipantIds);

                    return e.CreatorId == userId || formattedParticipantIds.Any(i => i == userId);
                })
                .ToList();

            IEnumerable<GroupConversationReadDto> result = _mapper.Map<IEnumerable<GroupConversationReadDto>>(found);

            foreach (GroupConversationReadDto item in result)
            {
                item.ParticipantIdsList = formattedParticipantIdsDictionary.First(e => e.Key == item.Id).Value;
            }

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Returns GroupConversation with provided id.
        /// </summary>
        /// <param name="id">Id of requested GroupConversation.</param>
        /// <response code="200">GroupConversation returned.</response>
        /// <response code="400">Non-existing GroupConversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>GroupConversation by id.</returns>
        [HttpGet("{id}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<GroupConversationReadDto> Get(int id)
        {
            GroupConversation found = _groupConversationRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "GroupConversation doesn't exist.");
            }

            GroupConversationReadDto result = _mapper.Map<GroupConversationReadDto>(found);

            List<int> formattedParticipantIds = FormatHelper.CsvToListInt(found.ParticipantIds);

            result.ParticipantIdsList = formattedParticipantIds;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Returns GroupConversation with provided id for User.
        /// </summary>
        /// <param name="id">Id of requested GroupConversation.</param>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="200">GroupConversation returned.</response>
        /// <response code="400">Non-existing User or GroupConversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of GroupConversation.</response>
        /// <response code="500">Internal server error.</response>
        /// <returns>GroupConversation by id for User.</returns>
        [HttpGet("{id}/for-user/{userId}")]
        [Authorization]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<GroupConversationReadDto> GetForUser(int id, int userId)
        {
            if (_userRepository.Get(userId) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User doesn't exist.");
            }

            GroupConversation found = _groupConversationRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "GroupConversation doesn't exist.");
            }

            List<int> formattedParticipantIds = FormatHelper.CsvToListInt(found.ParticipantIds);

            if (found.CreatorId != userId && !formattedParticipantIds.Any(e => e == userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You can't access GroupConversations you're not a part of.");
            }

            GroupConversationReadDto result = _mapper.Map<GroupConversationReadDto>(found);

            result.ParticipantIdsList = formattedParticipantIds;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Deletes GroupConversation.
        /// </summary>
        /// <param name="id">Id of requested GroupConversation.</param>
        /// <response code="204">Deletes GroupConversation with provided id.</response>
        /// <response code="400">Non-existing GroupConversation.</response>
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
            _groupConversationRepository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes GroupConversations for User.
        /// </summary>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="204">Deletes GroupConversations that belong to User with provided id.</response>
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

            Dictionary<int, List<int>> formattedParticipantIdsDictionary = new Dictionary<int, List<int>>();

            List<GroupConversation> found = _groupConversationRepository.Get()
                .Where(e =>
                {
                    List<int> formattedParticipantIds = FormatHelper.CsvToListInt(e.ParticipantIds);

                    formattedParticipantIdsDictionary.Add(e.Id, formattedParticipantIds);

                    return e.CreatorId == userId || formattedParticipantIds.Any(i => i == userId);
                })
                .ToList();

            foreach (GroupConversation entity in found)
            {
                if (entity.CreatorId == userId)
                {
                    _groupConversationRepository.Delete(id);
                }
                else
                {
                    List<int> participantIds = formattedParticipantIdsDictionary[entity.Id];

                    participantIds.Remove(userId);

                    entity.ParticipantIds = FormatHelper.ListIntToCsv(participantIds);

                    _groupConversationRepository.Update(entity);
                }
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        /// <summary>
        /// Deletes GroupConversation for User.
        /// </summary>
        /// <param name="id">Id of requested GroupConversation.</param>
        /// <param name="userId">Id of requested User.</param>
        /// <response code="204">Deletes GroupConversation with provided Id and checks if it belongs to User with provided id.</response>
        /// <response code="400">Non-existing User or GroupConversation.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">User is not a part of GroupConversation.</response>
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

            GroupConversation found = _groupConversationRepository.Get(id);

            if (found == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "GroupConversation doesn't exist.");
            }

            List<int> formattedParticipantIds = FormatHelper.CsvToListInt(found.ParticipantIds);

            if (found.CreatorId != userId && !formattedParticipantIds.Any(e => e == userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You can't access GroupConversations you're not a part of.");
            }

            if (found.CreatorId == userId)
            {
                _groupConversationRepository.Delete(id);
            }
            else
            {
                formattedParticipantIds.Remove(userId);

                found.ParticipantIds = FormatHelper.ListIntToCsv(formattedParticipantIds);

                _groupConversationRepository.Update(found);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
