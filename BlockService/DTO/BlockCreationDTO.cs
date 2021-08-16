using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.DTO
{
    /// <summary>
    /// DTO creation model za blokiranje korisnika
    /// </summary>
    public class BlockCreationDTO
    {

        /// <summary>
        /// ID korisnika koji blokira
        /// </summary>
        [Required(ErrorMessage = "Blocker ID is required!")]
        public int blockerID { get; set; }


        /// <summary>
        /// ID korisnika koji se blokira
        /// </summary>
        [Required(ErrorMessage = "Blocked ID is required!")]
        public int blockedID { get; set; }
    }
}
