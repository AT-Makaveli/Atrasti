using FirebaseAdmin.Messaging;

namespace Atrasti.API.Services.Firebase
{
    public interface IFirebaseMessageSerializer
    {
        Message Serialize();
    }
}