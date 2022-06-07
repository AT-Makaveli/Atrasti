using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Atrasti.Search;
using Newtonsoft.Json;

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
            await SyncProducts();
        }

        private async Task TestSearch()
        {
            await _searchService.RemoveAllDocuments("companies");
            //await _searchService.RemoveAllDocuments("products");
            //IReadOnlyCollection<SearchDocument> documents = await _searchService.Search("fasdf asdf");
            
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
            command.CommandText = "SELECT p.Id, p.Title, p.Description, p.Tags, u.Id as UserId, bc.Title as Category from products p JOIN Users u ON p.CompanyId = u.Id JOIN BaseCategories bc on p.ProductCategory = bc.Id;";
            DbDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string[] tags = JsonConvert.DeserializeObject<string[]>(reader.GetData<string>("Tags"));
                bool result = await _searchService.IndexDocument("products", new SearchDocument()
                {
                    DocumentId = reader.GetData<uint>("Id").ToString(),
                    CompanyId = reader.GetData<int>("UserId"),
                    Title = reader.GetData<string>("Title"),
                    Description = reader.GetData<string>("Description"),
                    Tags = tags,
                    Category = reader.GetData<string>("Category"),
                    BusinessType = "",
                    MainProducts = "",
                    MainMarkets = "",
                    Certificates = "",
                    City = "",
                    State = "",
                    Country = "",
                });
            }
        }
    }
}