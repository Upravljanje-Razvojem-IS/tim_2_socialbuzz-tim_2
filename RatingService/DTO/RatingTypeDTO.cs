using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    /// <summary>
    /// DTO  model tipa ocene
    /// </summary>
    public class RatingTypeDTO
    {
        /// <summary>
        /// ID tipa ocene
        /// </summary>
        public int RatingTypeID { get; set; }

        /// <summary>
        /// naziv tipa ocene
        /// </summary>
        public String RatingTypeName { get; set; }
    }
}
