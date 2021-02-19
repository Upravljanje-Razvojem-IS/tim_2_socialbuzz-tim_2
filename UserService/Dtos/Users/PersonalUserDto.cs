using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    /// <summary>
    /// DTO class which models personal users
    /// </summary>
    public class PersonalUserDto: UserDto
    {
        /// <summary>
        /// User's first and last name
        /// </summary>
        public String FirstAndLastName { get; set; }
    }
}
