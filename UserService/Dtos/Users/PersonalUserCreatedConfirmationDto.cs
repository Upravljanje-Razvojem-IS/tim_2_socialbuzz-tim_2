using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos.Users
{
    public class PersonalUserCreatedConfirmationDto: UserCreatedConfirmationDto
    {
        /// <summary>
        /// User's first and last name
        /// </summary>
        public String FirstAndLastName { get; set; }
    }
}
