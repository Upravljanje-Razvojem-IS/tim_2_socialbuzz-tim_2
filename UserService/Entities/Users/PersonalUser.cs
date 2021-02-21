using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entity class which models personal user of the system
    /// </summary>
    public class PersonalUser: User
    {
        /// <summary>
        /// User's first name
        /// </summary>
        [Column("FirstName")]
        [StringLength(50)]
        [Required]
        public String FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [Column("LastName")]
        [StringLength(50)]
        [Required]
        public String LastName { get; set; }
    }
}
