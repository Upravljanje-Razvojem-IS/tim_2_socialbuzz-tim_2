using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Dtos
{
    /// <summary>
    /// DTO class which models user and his account in the system
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Boolean value which indicates if user's account
        /// is active
        /// </summary>
        public Boolean IsActive { get; set; }

        /// <summary>
        /// Users telephone
        /// </summary>
        public String Telephone { get; set; }

        /// <summary>
        /// Username of the user's account
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// User's role in the system 
        /// used for authorization
        /// </summary>
        public String Role { get; set; }

        /// <summary>
        /// User's city 
        /// </summary>
        public String City { get; set; }
    }
}
