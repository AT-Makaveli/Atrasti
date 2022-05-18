using System;
using System.Threading.Tasks;
using Atrasti.Integrations.ElasticSearch;
using Atrasti.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atrasti.Integrations
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.ConfigureSearchModule();
            serviceCollection.AddSingleton<DbConnectionProvider>();
            serviceCollection.AddSingleton<ElasticSearchIntegration>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            await SyncElasticSearch(serviceProvider);

            Console.WriteLine("Finished integrations! Press key to exit.");
            Console.ReadKey();
        }

        private static Task SyncElasticSearch(IServiceProvider serviceProvider)
        {
            ElasticSearchIntegration searchIntegration = serviceProvider.GetRequiredService<ElasticSearchIntegration>();

            return searchIntegration.IntegrateAsync();
        }
    }
}