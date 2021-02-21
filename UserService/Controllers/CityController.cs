using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Dtos;

namespace UserService.Controllers
{
    /// <summary>
    /// Controller with endpoints for fetching, creating, updating
    /// and deleting cities
    /// </summary>
    [ApiController]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        private readonly IMapper mapper;

        public CityController(ICityRepository cityRepository, IMapper mapper)
        {
            this.cityRepository = cityRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns list of all cities in the system
        /// </summary>
        /// <param name="cityName">Name of the city</param>
        /// <returns>List of cities</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No cities  are found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<CityDto>> GetCities(string cityName)
        {
            var cities = cityRepository.GetCities(cityName);
            if (cities == null || cities.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<CityDto>>(cities));
        }

        /// <summary>
        /// Returns city with cityId
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns>City with cityId</returns>
        ///<response code="200">Returns the city</response>
        /// <response code="404">City with cityId is not found</response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CityDto> GetRoleById(Guid cityId)
        {
            var city = cityRepository.GetCityByCityId(cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CityDto>(city));
        }

    }
}
