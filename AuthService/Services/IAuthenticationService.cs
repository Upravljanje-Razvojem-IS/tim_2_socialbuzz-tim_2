using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> LoginAsync(string email, string password);
    }
}
