using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class PrivateTokenRequest
    {
        public Guid PublicToken { get; set; }
    }
}
