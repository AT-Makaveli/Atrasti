using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Atrasti.API.Models.Config;
using Atrasti.API.Models.Token;
using Atrasti.API.Services.Refresh;
using Atrasti.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace Atrasti.API.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshService _refreshService;

        public JwtService(JwtSettings jwtSettings, IRefreshService refreshService)
        {
            _jwtSettings = jwtSettings;
            _refreshService = refreshService;
        }

        public UserToken GenerateUserToken(AtrastiUser user)
        {
            UserToken userToken = new UserToken();
            
            DateTime expireTime = DateTime.UtcNow.AddDays(1);
            userToken.ValidTo = expireTime.TimeOfDay;
            
            var jwtToken = new JwtSecurityToken(issuer: _jwtSettings.ValidIssuer, audience: _jwtSettings.ValidAudience,
                claims: GetClaims(user), notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(expireTime).DateTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey)),
                    SecurityAlgorithms.HmacSha256));

            userToken.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            userToken.RefreshToken = _refreshService.GenerateRefreshToken();

            return userToken;
        }
        
        private IEnumerable<Claim> GetClaims(AtrastiUser atrastiUser)
        {
            IEnumerable<Claim> claims = new Claim[]
            {
                new(JwtClaims.Id, atrastiUser.Id.ToString()),
                new(JwtClaims.NameIdentifier, atrastiUser.Id.ToString()),
                new(JwtClaims.Name, atrastiUser.UserName),
                new(JwtClaims.Email, atrastiUser.Email),
                new(JwtClaims.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };
            return claims;
        }
    }
}