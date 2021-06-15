

using OcelotGateway.Models;
using OcelotGateway.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace OcelotGateway.Handlers
{
    public class AccessTokenHandler: DelegatingHandler
    {
        private readonly IAuthService _authService;
        public AccessTokenHandler(IAuthService authService)
        {
            _authService = authService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AuthenticationResponse response = _authService.getAccessToken("").Result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
            //do stuff and optionally call the base handler..
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
