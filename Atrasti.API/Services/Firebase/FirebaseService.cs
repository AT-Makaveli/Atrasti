using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Atrasti.API.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseApp _app;
        private readonly FirebaseMessaging _messaging;

        public FirebaseService()
        {
            _app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(
                    "/home/eduard/Skrivbord/Atrasti/Atrasti.API/atrasti-test-firebase-adminsdk-qbyuy-2cde979fe5.json")
            });
            
            _messaging = FirebaseMessaging.GetMessaging(_app);
        }

        public Task<BatchResponse> SendMessages(IEnumerable<IFirebaseMessageSerializer> messageSerializers)
        {
            var messagesToSend = messageSerializers
                .Select(firebaseMessageSerializer => firebaseMessageSerializer.Serialize()).ToList();

            if (messagesToSend.Count > 0)
                return FirebaseMessaging.DefaultInstance.SendAllAsync(messagesToSend);

            return Task.FromResult<BatchResponse>(null);
        }

        public Task<string> SendMessage(IFirebaseMessageSerializer messageSerializer)
        {
            return _messaging.SendAsync(messageSerializer.Serialize());
        }
    }
}