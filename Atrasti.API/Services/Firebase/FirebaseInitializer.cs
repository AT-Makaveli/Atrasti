using Microsoft.Extensions.DependencyInjection;

namespace Atrasti.API.Services.Firebase
{
    public static class FirebaseInitializer
    {
        public static void SetupFirebase(this IServiceCollection collection)
        {
            collection.AddSingleton<IFirebaseService, FirebaseService>();
        }
    }
}