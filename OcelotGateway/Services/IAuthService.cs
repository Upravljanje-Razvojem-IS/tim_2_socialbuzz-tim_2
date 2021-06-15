using OcelotGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Services
{
    public interface IAuthService
    {
        public Task<AuthenticationResponse> getAccessToken(string publicToken);
    }
}
