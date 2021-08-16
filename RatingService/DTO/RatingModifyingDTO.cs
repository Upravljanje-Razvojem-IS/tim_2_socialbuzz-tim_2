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
        /// ID ocene koja se modifikuje
        /// </summary>
        public Guid RatingID { get; set; }

        /// <summary>
        /// ID objave za kojoj se nalazi ocena koja se modifikuje
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// ID tipa ocene
        /// </summary>
        [Required(ErrorMessage = "Type of rating is required!")]
        public int RatingTypeID { get; set; }

        /// <summary>
        /// datum ocene
        /// </summary>
        public DateTime RatingDate { get; set; }

        /// <summary>
        /// opis ocene
        /// </summary>
        public String RatingDescription { get; set; }
    }
}
