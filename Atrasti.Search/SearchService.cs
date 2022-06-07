using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace Atrasti.Search
{
    public class SearchService
    {
        private readonly ElasticClientFactory _clientFactory;

        public SearchService(ElasticClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyCollection<SearchDocument>> Search(string searchQuery)
        {
            ElasticClient client = _clientFactory.Create();
            var searchResponse = await client.SearchAsync<SearchDocument>(s => s
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(fields => fields
                            .Field(f => f.Title)
                            .Field(f => f.Tags)
                            .Field(f => f.Description)
                            .Field(f => f.City)
                            .Field(f => f.State)
                            .Field(f => f.Country)
                            .Field(f => f.BusinessType)
                            .Field(f => f.MainProducts)
                            .Field(f => f.MainMarkets)
                            .Field(f => f.Certificates)
                            .Field(f => f.Category)
                        )
                        .Query(searchQuery))
                ).Index(new[] {"companies", "products"})
            );

            return searchResponse.Documents;
        }

        public async Task<IReadOnlyCollection<SearchDocument>> SearchProducts(string index, string searchQuery)
        {
            ElasticClient client = _clientFactory.Create();
            var searchResponse = await client.SearchAsync<SearchDocument>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(sd => sd.Tags).Query(searchQuery)
                    )
                ).Index(index)
            );

            return searchResponse.Documents;
        }

        public async Task<bool> IndexDocument(string index, SearchDocument searchDocument)
        {
            ElasticClient client = _clientFactory.Create();
            var indexResponse =
                await client.IndexAsync(searchDocument, r => r.Id(searchDocument.DocumentId).Index(index));

            return indexResponse.IsValid;
        }

        public async Task<bool> RemoveDocument(string index, string documentId)
        {
            ElasticClient client = _clientFactory.Create();
            DeleteRequest<SearchDocument> searchDocument = new DeleteRequest<SearchDocument>(index, documentId);
            var deleteResponse = await client.DeleteAsync(searchDocument);

            return deleteResponse.IsValid;
        }

        public async Task RemoveAllDocuments(string index)
        {
            ElasticClient client = _clientFactory.Create();
            await client.DeleteByQueryAsync<SearchDocument>(del => del
                .Query(q =>
                    q.QueryString(
                        qs => qs.Query("*")
                    )
                ).Index(index)
            );
        }
    }
}