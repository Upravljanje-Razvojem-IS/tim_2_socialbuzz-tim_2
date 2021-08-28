using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactionService.Dtos;
using ReactionService.Entities;
using ReactionService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ReactionService.Controllers
{
    [Route("api/reaction-type")]
    [ApiController]
    public class ReactionTypesController : ControllerBase
    {
        private readonly IReactionTypeRepository _repository;
        private readonly IMapper _mapper;

        public ReactionTypesController(
            IReactionTypeRepository reactionRepository,
            IMapper mapper
        )
        {
            _repository = reactionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new ReactionType.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>New ReactionType.</returns>
        /// <response code="201">Creates new ReactionType.</response>
        /// <response code="400">Invalid value in body.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<ReactionTypeReadDto> Create([FromBody] ReactionTypeCreateDto request)
        {
            ReactionType newEntity = _mapper.Map<ReactionType>(request);

            newEntity = _repository.Create(newEntity);

            return StatusCode(StatusCodes.Status201Created, _mapper.Map<ReactionTypeReadDto>(newEntity));
        }

        /// <summary>
        /// Returns all ReactionTypes.
        /// </summary>
        /// <returns>All ReactionTypes.</returns>
        /// <response code="200">ReactionTypes returned.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public ActionResult<IEnumerable<ReactionTypeReadDto>> Get()
        {
            List<ReactionType> result = _repository.Get().ToList();

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<IEnumerable<ReactionTypeReadDto>>(result));
        }

        /// <summary>
        /// Returns ReactionType with provided id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ReactionType by id.</returns>
        /// <response code="200">ReactionType returned.</response>
        /// <response code="400">Non-existing ReactionType.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public ActionResult<ReactionTypeReadDto> Get(int id)
        {
            ReactionType result = _repository.Get(id);

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ReactionTypeReadDto>(result));
        }

        /// <summary>
        /// Updates ReactionType.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns>Updated ReactionType.</returns>
        /// <response code="200">ReactionType updated.</response>
        /// <response code="400">Invalid value in body or Non-existing ReactionType.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public ActionResult<ReactionTypeReadDto> Update(int id, [FromBody] ReactionTypeUpdateDto request)
        {
            ReactionType found = _repository.Get(id);

            found = _mapper.Map(request, found);

            _repository.Update(found);

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ReactionTypeReadDto>(found));
        }

        /// <summary>
        /// Deletes ReactionType.
        /// </summary>
        /// <param name="id">Id of requested ReactionType.</param>
        /// <returns></returns>
        /// <response code="204">Deletes ReactionType with provided id.</response>
        /// <response code="400">Non-existing ReactionType.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
