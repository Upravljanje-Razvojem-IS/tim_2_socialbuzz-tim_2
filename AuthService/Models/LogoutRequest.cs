using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class LogoutRequest
    {
        public Guid UserId { get; set; }
    }
}
