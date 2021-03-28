using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos.Users
{
    public class UserUpdateDto
    {
        //Email and password shouldn't be updated

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
        /// Foreign key cityId
        /// </summary>
        public Guid CityId { get; set; }
    }
}
