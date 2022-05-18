using System;
using Microsoft.Extensions.Configuration;
using Nest;

namespace Atrasti.Search
{
    public class ElasticClientFactory
    {
        private readonly IConfiguration _configuration;

        public ElasticClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticClient Create()
        {
            string url = _configuration.GetSection("Elastic")["URL"];
            string userName = _configuration.GetSection("Elastic")["UserName"];
            string password = _configuration.GetSection("Elastic")["Password"];
            string apiKey = _configuration.GetSection("Elastic")["ApiKey"];
            
            var settings =
                new ConnectionSettings(
                        new Uri(url))
                    .BasicAuthentication(userName, password)
                    .RequestTimeout(TimeSpan.FromMinutes(2));

            return new ElasticClient(settings);
        }
    }
}