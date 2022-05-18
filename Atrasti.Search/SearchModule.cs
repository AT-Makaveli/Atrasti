using Microsoft.Extensions.DependencyInjection;

namespace Atrasti.Search
{
    public static class SearchModule
    {
        public static void ConfigureSearchModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ElasticClientFactory>();
            serviceCollection.AddTransient<SearchService>();
        }
    }
}