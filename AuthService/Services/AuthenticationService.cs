using AuthService.Entities;
using AuthService.Models;
using AuthService.Options;
using AuthService.Repositories;
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
        private readonly IAuthInfoRepository _authInfoRepository;

        public AuthenticationService(IConfiguration configuration, JwtSettings jwtSettings, IAuthInfoRepository authInfoRepository)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
            _authInfoRepository = authInfoRepository;
        }

        public AuthenticationResponse GetAccessToken(Guid publicToken)
        {
            AuthInfo authInfo =_authInfoRepository.GetAuthInfoByPublicToken(publicToken);
            if(authInfo != null)
            {
                //TODO: user already has private token
                string privateToken = IssueToken(authInfo.UserId.ToString(), authInfo.Role);
                authInfo.PrivateToken = privateToken;
                _authInfoRepository.SaveChanges();
                return new AuthenticationResponse
                {
                    Token = privateToken,
                    Succes = true
                };
            }
            return new AuthenticationResponse
            {
                Error = "Public token not found",
                Succes = false
            };
        }

        public AuthInfo GetAuthInfoByPublicToken(Guid publicToken)
        {
            return _authInfoRepository.GetAuthInfoByPublicToken(publicToken);
        }

        public AuthInfo GetAuthInfoByUserId(Guid id)
        {
            return _authInfoRepository.GetAuthInfoByUserId(id);
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
                        Error = res.Message.ToString() 
                    };
                }

                string publicToken = Guid.NewGuid().ToString();
                Guid id = res.AccountInfo.Id;
                string role = res.AccountInfo.Role;
                DateTime dateIssued = DateTime.UtcNow;
                AuthInfo user = _authInfoRepository.GetAuthInfoByUserId(id);
                if(user != null)
                {
                    return new AuthenticationResponse
                    {
                        Token = user.PublicToken,
                        Succes = true
                    };
                }
                AuthInfo authInfo = new AuthInfo
                {
                    UserId = id,
                    Role = role,
                    PublicToken = publicToken,
                    TimeOfIssuingPublicToken = dateIssued
                };
                _authInfoRepository.CreateAuthInfo(authInfo);
                return new AuthenticationResponse
                {
                    Token = publicToken,
                    Succes = true
                };               
            }

        }

        public void Logout(Guid userId)
        {
            _authInfoRepository.DeleteAuthInfo(userId);
        }

        private string IssueToken(string userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                            new Claim("id", userId),
                            new Claim(ClaimTypes.Role, role)
                        }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
