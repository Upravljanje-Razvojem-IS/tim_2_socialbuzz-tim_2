using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class Corporation: User
    {
        [Column("CorporationName")]
        [StringLength(50)]
        [Required]
        public String CorporationName { get; set; }
        [Column("Pib")]
        [StringLength(50)]
        [Required]
        public String Pib { get; set; }
        [Column("HeadquartersCity")]
        [StringLength(50)]
        [Required]
        public String HeadquartersCity { get; set; }
        [Column("HeadquartersAddress")]
        [StringLength(50)]
        [Required]
        public String HeadquartersAddress { get; set; }
    }
}
