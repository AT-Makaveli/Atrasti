using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Config;
using Atrasti.Data.Core;
using Atrasti.Data.Models;

namespace Atrasti.API.Services.Refresh
{
    public class RefreshService : IRefreshService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshService(IUserRepository userRepository, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken, string jwtToken)
        {
            if (refreshToken == null)
                throw new InvalidOperationException("/refresh: Invalid refresh token");
            
            AtrastiUser user = await _userRepository.FindUserWithUserDataByRefreshToken(refreshToken);
            if (user == null)
                throw new InvalidOperationException("/refresh: Invalid refresh token");

            if (string.IsNullOrEmpty(jwtToken))
                throw new InvalidOperationException("/refresh: Invalid jwt token");

            //This will validate the jwt token.
            ClaimsPrincipal principal = JwtHelpers.GetPrincipalFromExpiredToken(jwtToken, _jwtSettings);

            return true;
        }
    }
}