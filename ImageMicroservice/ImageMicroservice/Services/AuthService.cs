using ImageMicroservice.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMicroservice.Services
{
    public class AuthService :IAuthService
    {
        private readonly IUserRepository _usersRepo;
        private readonly IConfigurationSection _secretKey;

        public AuthService(IUserRepository usersRepo, IConfiguration config)
        {
            _usersRepo = usersRepo;
            _secretKey = config.GetSection("SecretKey");
        }

        public string Login(string username, string password)
        {
            if(_usersRepo.AreCredentialsValid(username, password))
            {

                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(500),
                    signingCredentials: signinCredentials
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return tokenString;
            }
            else
            {
                throw new Exception("Invalid credentials");
            }
           
        }

    
    }
}
