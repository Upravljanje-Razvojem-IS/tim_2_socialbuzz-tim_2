using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Entities

{
    public class User
    {
        [Column("UserId")]
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid UserId { get; set; }

        [Column("Email")]
        [StringLength(50)]
        [Required]
        public String Email { get; set; }

        [Column("Password")]
        [StringLength(50)]
        [Required]
        public String Password { get; set; }

        [Column("IsActive")]
        [Required]
        public Boolean IsActive { get; set; }

        [Column("Telephone")]
        [StringLength(30)]
        [Required]
        public String Telephone { get; set; }

        [Column("Username")]
        [StringLength(50)]
        [Required]
        public String Username { get; set;  }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
