using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

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
        public AccountInfoDto AccountInfo { get; set; }


        public CheckAccountResponse()
        {

        }

        public CheckAccountResponse(bool succes, string message, AccountInfoDto account)
        {
            this.Succes = succes;
            this.Message = message;
            this.AccountInfo = account;
        }
    }
}
