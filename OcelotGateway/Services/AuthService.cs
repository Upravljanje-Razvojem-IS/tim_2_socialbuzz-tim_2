using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OcelotGateway.Models;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace OcelotGateway.Services
{

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> getAccessToken(string publicToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{ _configuration["Services:AuthService"] }api/auth");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(publicToken));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                AuthenticationResponse res = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
                if (!response.IsSuccessStatusCode)
                {

                }
                return res;
            }

        }
    }
}
