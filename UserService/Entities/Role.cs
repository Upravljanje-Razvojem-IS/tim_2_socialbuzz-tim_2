using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entity class which models roles in the system,
    /// which are used for authorization 
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique identifier for the role
        /// </summary>
        [Column("RoleId")]
        [Key]
        [Required]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        [Column("RoleName")]
        [StringLength(50)]
        [Required]
        public String RoleName { get; set; }
    }
}
