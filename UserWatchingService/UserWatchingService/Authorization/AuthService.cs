using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserWatchingService.CustomException;
using UserWatchingService.Data;
using UserWatchingService.Entities;
using UserWatchingService.Interfaces;

namespace UserWatchingService.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly DataAccessLayer _context;
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config, DataAccessLayer context)
        {
            _config = config;
            _context = context;
        }

        public string Login(Principal principal)
        {
            var coorporate = _context.Users.FirstOrDefault(e => e.Email == principal.Email && e.Password == principal.Password);

            if (coorporate == null)
                throw new UserException("User does not exist");

            return this.GenerateJwt("Coorporate");
        }

        public string GenerateJwt(string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                             _config["Jwt:Issuer"],
                                             new[] { new Claim("role", role) },
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
