using LoginAuth.Models;
using LoginAuth.Models.Constants;
using LoginAuth.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LoginAuth.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private IConfiguration _config;

        public AuthenticateService(IConfiguration config)
        {
            _config = config;
        }

        public UserModel Authenticate(LoginViewModel model)
        {
            var currentUser = Users.UserData.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (currentUser != null)
                return currentUser;

            return null;
        }

        public string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , claims
                , expires: DateTime.Now.AddMinutes(1)
                , signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
