using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class AuthenticationRespose
    {
        /// <summary>
        /// A generated token after successful authentication
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Bool value which indicates if autentication is successful
        /// </summary>
        public bool Succes { get; set; }

        /// <summary>
        /// List of authentication errors 
        /// </summary>
        public IEnumerable<string> Errors { get; set; }
    }
}
