using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class AccountInfo: IdentityUser<Guid>
    {
        /// <summary>
        /// Indicator whether the user account is active or disabled
        /// </summary>
        public bool AccountIsActive { get; set; } = true;

        private AccountInfo(): base()
        {

        }

        public AccountInfo(string username, string email) : base(username)
        {
            this.UserName = username;
            this.Email = email;
        }

        public AccountInfo(string username, string email, Guid id) : base(username)
        {
            this.Id = id;
            this.UserName = username;
            this.Email = email;
        }
    }
}
