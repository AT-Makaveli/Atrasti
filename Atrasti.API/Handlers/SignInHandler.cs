using System.Threading.Tasks;
using Atrasti.API.Services.Firebase;
using Atrasti.Data.Core;

namespace Atrasti.API.Handlers
{
    public class SignInHandler
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUserRepository _userRepository;

        public SignInHandler(IFirebaseService firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
        }

        public Task SignInUser()
        {
            return Task.CompletedTask;
        }
    }
}