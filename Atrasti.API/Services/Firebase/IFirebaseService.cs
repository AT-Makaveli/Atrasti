using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace Atrasti.API.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<BatchResponse> SendMessages(IEnumerable<IFirebaseMessageSerializer> messageSerializers);

        Task<string> SendMessage(IFirebaseMessageSerializer messageSerializer);
    }
}