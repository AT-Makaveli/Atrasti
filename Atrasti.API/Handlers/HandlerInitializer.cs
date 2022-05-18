using Microsoft.Extensions.DependencyInjection;

namespace Atrasti.API.Handlers
{
    public static class HandlerInitializer
    {
        public static void SetupHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<SignInHandler>();
        }
    }
}