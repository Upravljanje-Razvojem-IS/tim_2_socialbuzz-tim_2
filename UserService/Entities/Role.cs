using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class Role
    {
        [Column("RoleId")]
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid RoleId { get; set; }

        [Column("RoleName")]
        [StringLength(50)]
        [Required]
        public String RoleName { get; set; }
    }
}
