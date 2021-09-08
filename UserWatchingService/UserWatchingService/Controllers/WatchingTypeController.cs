using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UserWatchingService.Dtos.WatchingTypeDto;
using UserWatchingService.Interfaces;

namespace UserWatchingService.Controllers
{
    [ApiController]
    [Route("api/watching-type")]
    [Authorize]
    public class WatchingTypeController : ControllerBase
    {
        private readonly IWatchingTypeRepository _repository;

        public WatchingTypeController(IWatchingTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all watching types
        /// </summary>
        /// <returns>list of watching types</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _repository.Get();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get watching type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>watching type</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Create watching type
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>new watching type</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult Post([FromBody] WatchingTypeCreateDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update watching type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>updated watching type</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, WatchingTypeCreateDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete watching type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
