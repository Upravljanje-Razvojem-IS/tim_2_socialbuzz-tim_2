using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CheckAccountRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
