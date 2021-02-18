using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class AuthFailResponse
    {
        /// <summary>
        /// List of authentication errors 
        /// </summary>
        public IEnumerable<string> Errors { get; set; }


    }
}
