using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UserWatchingService.Dtos.WatchingDto;
using UserWatchingService.Interfaces;

namespace UserWatchingService.Controllers
{
    [ApiController]
    [Route("api/watching")]
    [Authorize]
    public class WatchingController : ControllerBase
    {
        private readonly IWatchingRepository _repository;

        public WatchingController(IWatchingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all watchings
        /// </summary>
        /// <returns>list of watchings</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult GetAll()
        {
            var entities = _repository.Get();

            if (entities.Count == 0)
                return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get watching by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>watching</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        /// <summary>
        /// Create watching
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>new watching</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public ActionResult Post([FromBody] WatchingCreateDto dto)
        {
            var entity = _repository.Create(dto);

            return Ok(entity);
        }

        /// <summary>
        /// Update watching
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>updated watching</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        public ActionResult Put(Guid id, WatchingCreateDto dto)
        {
            var entity = _repository.Update(id, dto);

            return Ok(entity);
        }

        /// <summary>
        /// Delete watching by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}
