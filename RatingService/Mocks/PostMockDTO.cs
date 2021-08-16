using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Mocks
{
    /// <summary>
    /// DTO  model za objave korisnika
    /// </summary>
    public class PostMockDTO
    {
        /// <summary>
        /// ID objave
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Naziv korisnikove objave
        /// </summary>
        public String PostTitle { get; set; }

        /// <summary>
        /// Opis korisnikove objave
        /// </summary>
        public String PostDescription { get; set; }

        /// <summary>
        /// ID korisnika koji je okacio objavu
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// ID condition za objavu korisnika
        /// </summary>
        public int ConditionID { get; set; }

        /// <summary>
        /// ID tipa objave kojem objava pripada
        /// </summary>
        public int PostTypeID { get; set; }

        /// <summary>
        /// Datum kada je korisnik postavio objavu
        /// </summary>
        public DateTime PostedOn { get; set; }
    }
}
