using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    /// <summary>
    /// DTO  modifikacija tipa ocene
    /// </summary>
    public class RatingTypeModifyingDTO
    {

        /// <summary>
        /// Naziv tipa ocene koji se modifikuje
        /// </summary>
        public String RatingTypeName { get; set; }
    }
}
