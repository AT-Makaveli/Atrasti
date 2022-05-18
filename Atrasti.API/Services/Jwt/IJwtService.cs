using Atrasti.API.Models.Token;
using Atrasti.Data.Models;

namespace Atrasti.API.Services.Jwt
{
    public interface IJwtService
    {
        UserToken GenerateUserToken(AtrastiUser user);
    }
}