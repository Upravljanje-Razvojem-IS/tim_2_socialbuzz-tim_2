using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    /// <summary>
    /// DTO za model za modifikaciju ocene
    /// </summary>
    public class RatingModifyingDTO
    {
        /// <summary>
        /// ID tipa ocene
        /// </summary>
        [Required(ErrorMessage = "Type of rating is required!")]
        public int RatingTypeID { get; set; }
    }
}
