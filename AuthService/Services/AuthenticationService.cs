using AuthService.Models;
using AuthService.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IConfiguration configuration, JwtSettings jwtSettings)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResponse> GetAccessToken(string publicToken)
        {
            using (HttpClient client = new HttpClient())
            {
                Principal principal = new Principal
                {
                    Email = "ramac@gmail.com",
                    Password = "pass123"
                };
                Uri url = new Uri($"{ _configuration["Services:UserService"] }api/accounts/checkPrincipal");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(principal));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                CheckPrincipalResponse res = await response.Content.ReadFromJsonAsync<CheckPrincipalResponse>();
                if (!response.IsSuccessStatusCode)
                {
                    return new AuthenticationResponse
                    {
                        Succes = false,
                        //TODO: get message from HttpResponseMessage
                        Error = res.Message.ToString()
                    };
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("id", res.AccountInfo.Id.ToString()),
                            new Claim(ClaimTypes.Role, res.AccountInfo.Role)
                        }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new AuthenticationResponse
                {
                    Token = tokenHandler.WriteToken(token),
                    Succes = true
                };
            }
        }

        public async Task<AuthenticationResponse> Login(Principal principal)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{ _configuration["Services:UserService"] }api/accounts/checkPrincipal");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(principal));
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                CheckPrincipalResponse res = await response.Content.ReadFromJsonAsync<CheckPrincipalResponse>();
                if (!response.IsSuccessStatusCode)
                {
                    return new AuthenticationResponse {
                        Succes = false,
                        //TODO: get message from HttpResponseMessage
                        Error = res.Message.ToString() };
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("id", res.AccountInfo.Id.ToString()),
                            new Claim(ClaimTypes.Role, res.AccountInfo.Role)
                        }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new AuthenticationResponse {
                    Token = tokenHandler.WriteToken(token),
                    Succes = true
                };
            }

        }
    }
}
