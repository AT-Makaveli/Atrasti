using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace Atrasti.Tests
{
    class Program
    {
        static async Task Main()
        {
            var settings =
                new ConnectionSettings(
                        new Uri("https://i-o-optimized-deployment-a922c0.es.ap-northeast-1.aws.found.io:9243"))
                    .BasicAuthentication("elastic", "7tS5q8V36D7RocjvA6diwsBg")
                    .RequestTimeout(TimeSpan.FromMinutes(2));

            var lowLevelClient = new ElasticClient(settings);

            //await TestIndex(lowLevelClient);
            //await TestSearch(lowLevelClient);
            await TestDelete(lowLevelClient);
            //PhoneticTest.Test();
        }

        private static async Task TestIndex(ElasticClient client)
        {
            var person = new Person
            {
                FirstName = "Martijn",
                LastName = "Nimanaj"
            };

            var indexResponse = await client.IndexAsync(person, r => r.Id(Guid.NewGuid().ToString()).Index("people"));
            Console.WriteLine(indexResponse.IsValid);
        }

        private static async Task TestDelete(ElasticClient client)
        {
            var response = await client.DeleteByQueryAsync<Person>(d =>
                d.Query(q =>
                    q.MatchAll()
                ).Index("products")
            );
        }
        
        private static async Task TestSearch(ElasticClient client)
        {

            var searchResponse = await client.SearchAsync<Person>(s => s
                .Query(q => q
                    .QueryString(s => 
                        s.Query("red"))
                ).Index("products")
            );
            /*var searchResponse = await lowLevelClient.SearchAsync<StringResponse>("people", PostData.Serializable(new
            {
                from = 0,
                size = 10,
                query = new
                {
                    match = new
                    {
                        FirstName = new
                        {
                            query = "Martijn"
                        }
                    }
                }
            }));*/

            foreach (Person documents in searchResponse.Documents)
            {
                Console.WriteLine(documents.FirstName);
                Console.WriteLine(documents.LastName);
                Console.WriteLine();
            }
        }

        class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}