using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    /// <summary>
    /// DTO za model ocene
    /// </summary>
    public class RatingDTO
    {
        /// <summary>
        /// ID ocene
        /// </summary>
        public Guid RatingID { get; set; }

        /// <summary>
        /// ID objave za koju se dodaje reakcija
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
