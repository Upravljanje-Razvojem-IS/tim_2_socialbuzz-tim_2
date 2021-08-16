using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService.Entities
{
    /// <summary>
    /// Model tabele blokiranja korisnika
    /// </summary>
    [Table("Block")]
    public class Block
    {
        /// <summary>
        /// ID bloka
        /// </summary>
        [Key]
        [Required]
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
