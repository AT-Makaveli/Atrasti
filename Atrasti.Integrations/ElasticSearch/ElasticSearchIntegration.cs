using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Atrasti.Search;

namespace Atrasti.Integrations.ElasticSearch
{
    public class ElasticSearchIntegration : IIntegration
    {
        private readonly SearchService _searchService;
        private readonly DbConnectionProvider _dbConnectionProvider;

        public ElasticSearchIntegration(SearchService searchService, DbConnectionProvider dbConnectionProvider)
        {
            _searchService = searchService;
            _dbConnectionProvider = dbConnectionProvider;
        }

        //TODO: Enable integrations with elastic search!
        public async Task IntegrateAsync()
        {
            //await TestSearch();
            await SyncCompanies();
        }

        private async Task TestSearch()
        {
            await _searchService.RemoveAllDocuments("companies");
            await _searchService.RemoveAllDocuments("products");
            IReadOnlyCollection<SearchDocument> documents = await _searchService.SearchProducts("companies", "Seb");
            
        }

        private async Task SyncCompanies()
        {
            DbConnection connection = await _dbConnectionProvider.ProvideConnection();
            DbCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT u.Id, u.Company, ci.*, c.*
                                    FROM Users u
                                    JOIN company_infos ci ON ci.RefId = u.Id
                                    JOIN companies c ON c.RefId = u.Id
                                    WHERE u.Company IS NOT NULL AND ci.RefId IS NOT NULL AND c.RefId IS NOT NULL;";
            
            DbDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool result = await _searchService.IndexDocument("companies", new SearchDocument()
                {
                    DocumentId = reader.GetData<int>("Id").ToString(),
                    CompanyId = reader.GetData<int>("Id"),
                    Title = reader.GetData<string>("Company"),
                    Description = reader.GetDataNullable<string>("CompanyDesc"),
                    BusinessType = reader.GetDataNullable<string>("BusinessType"),
                    MainProducts = reader.GetDataNullable<string>("MainProducts"),
                    MainMarkets = reader.GetDataNullable<string>("MainMarkets"),
                    Certificates = reader.GetDataNullable<string>("Certificates"),
                    City = reader.GetDataNullable<string>("City"),
                    State = reader.GetDataNullable<string>("State"),
                    Country = reader.GetDataNullable<string>("Country"),
                });
            }
        }
        
        private async Task SyncProducts()
        {
            DbConnection connection = await _dbConnectionProvider.ProvideConnection();
            DbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT u.Id, u.Company FROM Users u WHERE u.Company IS NOT NULL";
            DbDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                bool result = await _searchService.IndexDocument("companies", new SearchDocument()
                {
                    DocumentId = reader.GetData<int>("Id").ToString(),
                    CompanyId = reader.GetData<int>("Id"),
                    Title = reader.GetData<string>("Company"),
                });
            }
        }
    }
}