using OcelotGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotGateway.Services
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResponse> getAccessToken(Guid publicToken);
    }
}
