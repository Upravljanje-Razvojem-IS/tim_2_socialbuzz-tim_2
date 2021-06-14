using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class CheckPrincipalResponse
    {
        public bool Succes { get; set; }

        /// <summary>
        ///Message if autentication shouldn't be successful
        /// </summary>
        public string Message { get; set; }
        public AccountInfoDto AccountInfo { get; set; }


        public CheckPrincipalResponse()
        {

        }
    }
}
