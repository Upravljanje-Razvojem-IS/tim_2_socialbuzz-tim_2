using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Entities
{
    /// <summary>
    /// Model tipa ocene na objavu
    /// </summary>
    [Table("RatingType")]
    public class RatingType
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
