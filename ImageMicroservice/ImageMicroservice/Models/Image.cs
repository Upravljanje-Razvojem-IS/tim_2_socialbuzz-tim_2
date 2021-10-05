using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMicroservice.Models
{
    /// <summary>
    /// predstavlja model slike
    /// </summary>
    public class Image
    {
        /// <summary>
        /// id slike
        /// </summary>
        public long ImageID { get; set; }
        /// <summary>
        /// putanja slike
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// id posta
        /// </summary>
        public long PostID { get; set; }

        /// <summary>
        /// post
        /// </summary>
        public Post Post { get; set; }
        /// <summary>
        /// id usera
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// user
        /// </summary>
        public User User { get; set; }
    }
}
