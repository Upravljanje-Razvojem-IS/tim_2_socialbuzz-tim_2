using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    /// <summary>
    /// DTO class which models the confirmation 
    /// of the creation of user's account
    /// </summary>
    public class UserCreatedConfirmationDto
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
        /// Users telephone
        /// </summary>
        public String Telephone { get; set; }

        /// <summary>
        /// Username of the user's account
        /// </summary>
        public String Username { get; set; }
    }
}
