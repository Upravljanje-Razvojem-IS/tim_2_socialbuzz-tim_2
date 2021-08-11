using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.DTO
{
    /// <summary>
    /// DTO za model blokiranja korisnika
    /// </summary>
    public class BlockDTO
    {
        /// <summary>
        /// ID blocka
        /// </summary>
        public Guid BlockID { get; set; }

        /// <summary>
        /// datum blokiranja
        /// </summary>
        public DateTime BlockDate { get; set; }


        /// <summary>
        /// ID korisnika koji blokira
        /// </summary>
        public int blockerID { get; set; }


        /// <summary>
        /// ID korisnika koji se blokira
        /// </summary>
        public int blockedID { get; set; }

    }
}
