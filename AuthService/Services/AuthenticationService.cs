﻿using AuthService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AuthenticationResponse> LoginAsync(string email, string password)
        {
            return new Task<AuthenticationResponse>(null);
        }
    }
}
