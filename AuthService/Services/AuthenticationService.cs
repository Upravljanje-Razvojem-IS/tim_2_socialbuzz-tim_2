using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationRespose> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
