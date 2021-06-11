using AuthService.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public AuthenticationResponse Login(Principal principal)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{ configuration["Services:UserService"] }api/checkAccount");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(principal));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return new AuthenticationResponse { 
                    Succes = false,
                    //TODO: get message from HttpResponseMessage
                    Error = response.RequestMessage.ToString()};
                }
                return new AuthenticationResponse {
                    Succes = true
                };
            }

        }
    }
}
