using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    public class RatingCreationDTO
    {
        /// <summary>
        /// ID objave na koju se dodaje reakcija
        /// </summary>
        [Required(ErrorMessage = "Post ID is required!")]
        public int PostID { get; set; }

        /// <summary>
        /// ID tipa ocene
        /// </summary>
        [Required(ErrorMessage = "Type of rating is required!")]
        public int RatingTypeID { get; set; }

        /// <summary>
        /// opis ocene
        /// </summary>
        public String RatingDescription { get; set; }

        /// <summary>
        /// ID korisnika koji dodaje ocenu
        /// </summary>
        [Required(ErrorMessage = "User ID is required!")]
        public int UserID { get; set; }
    }
}
