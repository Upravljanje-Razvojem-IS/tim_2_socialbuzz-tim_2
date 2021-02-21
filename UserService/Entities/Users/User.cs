using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Entities
{
    /// <summary>
    /// Entity class which models user and his account in the system
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        [Column("UserId")]
        [Key]
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        [Column("Email")]
        [StringLength(50)]
        [Required]
        public String Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Column("Password")]
        [StringLength(50)]
        [Required]
        public String Password { get; set; }

        /// <summary>
        /// Boolean value which indicates if user's account
        /// is active
        /// </summary>
        [Column("IsActive")]
        [Required]
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Users telephone
        /// </summary>
        [Column("Telephone")]
        [StringLength(30)]
        [Required]
        public String Telephone { get; set; }

        /// <summary>
        /// Username of the user's account
        /// </summary>
        [Column("Username")]
        [StringLength(50)]
        [Required]
        public String Username { get; set; }

        /// <summary>
        /// User's role in the system 
        /// used for authorization
        /// </summary>
        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        /// <summary>
        /// User's city 
        /// </summary>
        [ForeignKey("CityId")]
        public Guid CityId { get; set; }
        public virtual City City { get; set; }
    }
}
