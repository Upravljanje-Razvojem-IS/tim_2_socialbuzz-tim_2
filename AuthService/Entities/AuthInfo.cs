using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Entities
{
    /// <summary>
    /// Entity class which models information about authenticated user 
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// Unique identifier for a city
        /// </summary>
        [Column("UserId")]
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// Unique identifier for a city
        /// </summary>
        [Column("TimeOfIssuingPublicToken")]
        [Required]
        public DateTime TimeOfIssuingPublicToken { get; set; }

        /// <summary>
        /// Public token issued for a user with UserId
        /// </summary>
        [Column("PublicToken")]
        [Required]
        public string PublicToken { get; set; }

        /// <summary>
        /// Private token issued for a user with UserId
        /// </summary>
        [Column("PrivateToken")]
        public string PrivateToken { get; set; }

        /// <summary>
        /// System role of the user with UserId
        /// </summary>
        [Column("Role")]
        [Required]
        public string Role { get; set; }
    }
}
