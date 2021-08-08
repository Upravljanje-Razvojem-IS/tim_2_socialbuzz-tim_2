using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Entities
{
    /// <summary>
    /// Model ocene na objavu
    /// </summary>
    [Table("Rating")]
    public class Rating
    {
        /// <summary>
        /// ID ocene
        /// </summary>
        [Key]
        [Required]
        public Guid RatingID { get; set; }
        /// <summary>
        /// ID objave na koju se dodaje ocena
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// ID tipa ocene
        /// </summary>
        public int RatingTypeID { get; set; }

        /// <summary>
        /// datum ocene
        /// </summary>
        public DateTime RatingDate { get; set; }

        /// <summary>
        /// opis ocene
        /// </summary>
        public String RatingDescription { get; set; }

        /// <summary>
        /// ID korisnika koji dodaje ocenu
        /// </summary>
        public int UserID { get; set; }

    }
}
