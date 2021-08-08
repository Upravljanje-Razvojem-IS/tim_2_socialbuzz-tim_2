using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Entities
{
    /// <summary>
    /// Model tabele agregacije podataka
    /// </summary>
    [Table("RatingAggregation")]
    public class RatingAggregation
    {
        /// <summary>
        /// ID agregacije
        /// </summary>
        [Key]
        [Required]
        public Guid RatingsAggregationID { get; set; }

        /// <summary>
        /// ID korisnika za koga se racuna statistika
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID tipa ocene za koju se gleda statistika
        /// </summary>
        public int RatingTypeID { get; set; }

        /// <summary>
        /// izracunata statistika
        /// </summary>
        public int NumberOfRaitings { get; set; }
    }
}
