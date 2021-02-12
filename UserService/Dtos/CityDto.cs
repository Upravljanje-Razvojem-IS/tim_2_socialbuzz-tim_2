using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    /// <summary>
    /// DTO class which models cities
    /// </summary>
    public class CityDto
    {
        /// <summary>
        /// Unique identifier for a city
        /// </summary>
        public Guid CityId { get; set; }

        /// <summary>
        /// City name 
        /// </summary>
        public String CityName { get; set; }
    }
}
