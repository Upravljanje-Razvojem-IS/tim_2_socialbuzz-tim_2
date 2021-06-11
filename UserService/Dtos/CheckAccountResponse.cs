using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CheckAccountResponse
    {
        /// <summary>
        /// Bool value which indicates if autentication should be successful
        /// </summary>
        public bool Succes { get; set; }

        /// <summary>
        ///Message if autentication shouldn't be successful
        /// </summary>
        public string Message { get; set; }


        public CheckAccountResponse()
        {

        }

        public CheckAccountResponse(bool succes, string message)
        {
            this.Succes = succes;
            this.Message = message;
        }
    }
}
