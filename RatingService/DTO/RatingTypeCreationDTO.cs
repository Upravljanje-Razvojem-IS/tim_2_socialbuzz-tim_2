using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    /// <summary>
    /// DTO za dodavanje novog tipa ocene na objavu
    /// </summary>
    public class RatingTypeCreationDTO
    {
        /// <summary>
        /// Naziv tipa ocene koja se kreira
        /// </summary>
        [Required(ErrorMessage = "Rating type is required!")]
        public String RatingTypeName { get; set; }
    }
}
