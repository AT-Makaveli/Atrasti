using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Atrasti.API.Models.Config;
using Microsoft.IdentityModel.Tokens;

namespace Atrasti.API.Helpers
{
    public static class JwtHelpers
    {
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtSettings jwtSettings)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = jwtSettings.ValidateAudience, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            
            return principal;
        }
    }
}