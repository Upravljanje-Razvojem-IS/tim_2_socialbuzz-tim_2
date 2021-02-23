using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos.Users
{
    public class PersonalUserUpdateDto: UserUpdateDto
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public String LastName { get; set; }
    }
}
