using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.DTO
{
    /// <summary>
    /// DTO modify model blokiranja korisnika
    /// </summary>
    public class BlockModifyingDTO
    {
        /// <summary>
        /// ID blocka
        /// </summary>
        public Guid BlockID { get; set; }

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
