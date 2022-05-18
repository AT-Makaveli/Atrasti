using System.Threading.Tasks;

namespace Atrasti.API.Services.Refresh
{
    public interface IRefreshService
    {
        string GenerateRefreshToken();

        Task<bool> ValidateRefreshToken(string refreshToken, string jwtToken);
    }
}