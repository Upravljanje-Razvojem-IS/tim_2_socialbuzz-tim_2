using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class PersonalUser: User
    {
        [Column("FirstName")]
        [StringLength(50)]
        [Required]
        public String FirstName { get; set; }

        [Column("LastName")]
        [StringLength(50)]
        [Required]
        public String LastName { get; set; }
    }
}
