using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public Boolean IsActive { get; set; }
        public String Telephone { get; set; }
        public String Username { get; set; }
        public String Role { get; set; }
        public String City { get; set; }
    }
}
