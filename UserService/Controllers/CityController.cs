using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Dtos;
using UserService.Dtos.Cities;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Services.Cities;

namespace UserService.Controllers
{
    /// <summary>
    /// Controller with endpoints for fetching, creating, updating
    /// and deleting cities
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly ICitiesService _citiesService;

        public CityController(ICityRepository cityRepository, IMapper mapper, LinkGenerator linkGenerator, ICitiesService citiesService)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            _citiesService = citiesService;
        }

        /// <summary>
        /// Returns list of all cities in the system
        /// </summary>
        /// <param name="cityName">Name of the city</param>
        /// <returns>List of cities</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No cities  are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<CityDto>> GetCities(string cityName)
        {
            try
            {
                var cities = _citiesService.GetCities(cityName);
                if (cities == null || cities.Count == 0)
                {
                    return NoContent();
                }
                return Ok(mapper.Map<List<CityDto>>(cities));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Returns city with cityId
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns>City with cityId</returns>
        ///<response code="200">Returns the city</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">City with cityId is not found</response>
        /// <response code="500">Error on the server while fetching cities</response>
        [AllowAnonymous]
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CityDto> GetCityById(Guid cityId)
        {
            try
            {
                var city = _citiesService.GetCityByCityId(cityId);
                if (city == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CityDto>(city));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //TODO: Provide example body
        /// <summary>
        /// Creates a new city
        /// </summary>
        /// <param name="city">Model of city</param>
        /// <returns>Confirmation of the creation of city</returns>
        /// <response code="200">Returns the created city</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [Authorize(Roles="Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<CityCreatedConfirmationDto> CreateCity([FromBody] CityCreationDto city)
        {
            try
            {
                City newCity = mapper.Map<City>(city);
                CityCreatedConfirmation createdCity = _citiesService.CreateCity(newCity);
                var location = linkGenerator.GetPathByAction("GetCityById", "City", new { cityId = createdCity.CityId });
                return Created(location, mapper.Map<CityCreatedConfirmationDto>(createdCity));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Updates city
        /// </summary>
        /// <param name="cityId">City's Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated city</response>
        /// <response code="400">City with cityId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server while updating</response>
        [Authorize(Roles="Admin")]
        [HttpPut("{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CityDto> UpdateCity([FromBody] CityUpdateDto cityUpdate, Guid cityId)
        {
            try
            {
                City cityWithId = _citiesService.GetCityByCityId(cityId);
                if (cityWithId == null)
                {
                    return NotFound();
                }
                City updatedCity = mapper.Map<City>(cityUpdate);
                updatedCity.CityId = cityId;
                mapper.Map(updatedCity, cityWithId);
                cityRepository.SaveChanges();
                return Ok(mapper.Map<CityDto>(cityWithId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");

            }
        }

        /// <summary>
        /// Deleting city with cityId
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">City succesfully deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">City with cityId not found</response>
        /// <response code="409">City referenced in another table</response>
        /// <response code="500">Error on the server while deleting</response>
        [Authorize(Roles="Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{cityId}")]
        public IActionResult DeleteCity(Guid cityId)
        {
            try
            {
                var city = _citiesService.GetCityByCityId(cityId);
                if (city == null)
                {
                    return NotFound();
                }
                _citiesService.DeleteCity(cityId);
                return NoContent();
            }
            catch(Exception ex)
            {
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintViolationException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

    }
}
