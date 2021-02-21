using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entity class which models corporation user of the system
    /// </summary>
    public class Corporation: User
    {
        /// <summary>
        /// Coropration name
        /// </summary>
        [Column("CorporationName")]
        [StringLength(50)]
        [Required]
        public String CorporationName { get; set; }

        /// <summary>
        /// Copropration PIB
        /// </summary>
        [Column("Pib")]
        [StringLength(50)]
        [Required]
        public String Pib { get; set; }

        /// <summary>
        /// City in which headquarters of corporation
        /// is located
        /// </summary>
        [Column("HeadquartersCity")]
        [StringLength(50)]
        [Required]
        public String HeadquartersCity { get; set; }

        /// <summary>
        /// Address of the location in which headquarters of the corporation
        /// is located
        /// </summary>
        [Column("HeadquartersAddress")]
        [StringLength(50)]
        [Required]
        public String HeadquartersAddress { get; set; }
    }
}
