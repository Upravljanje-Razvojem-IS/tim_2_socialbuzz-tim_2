using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OcelotGateway.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using AuthService.Models;

namespace OcelotGateway.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> getAccessToken(Guid publicToken)
        {
            using (HttpClient client = new HttpClient())
            {
                PrivateTokenRequest body = new PrivateTokenRequest
                {
                    PublicToken = publicToken
                };
                Uri url = new Uri($"{ _configuration["Services:AuthService"] }api/auth");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(body));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                AuthenticationResponse res = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
                return res;
            }

        }
    }
}
