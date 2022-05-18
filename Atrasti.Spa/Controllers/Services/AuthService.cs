using Atrasti.Spa.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Atrasti.Data.Models;

namespace Atrasti.Spa.Controllers.Services
{
    public class AuthService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly int _jwtLifespan;
        
        public AuthService(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtLifespan = 120;
        }
        
        public AuthData GetAuthData(AtrastiUser userModel)
        {
            string token = GenerateJsonWebToken(userModel.Id.ToString());

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)DateTime.Now.AddMinutes(120)).ToUnixTimeSeconds(),
                Id = userModel.Id.ToString()
            };
        }

        private string GenerateJsonWebToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtIssuer,
              _jwtIssuer,
              new List<Claim> {
                  new Claim(ClaimTypes.Name, userId)
              },
              expires: DateTime.Now.AddMinutes(_jwtLifespan),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}