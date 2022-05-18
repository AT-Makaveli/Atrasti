using Atrasti.Data.Models;
using Dapper;
using System.Threading.Tasks;

namespace Atrasti.Data.Tables.App
{
    internal class CompanyTable
    {
        private readonly ConnectionProvider _connectionProvider;

        public CompanyTable(ConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<Company> FindCompanyByIdAsync(int id)
        {
            var connection = await _connectionProvider.Connection();
            return connection.QuerySingleOrDefault<Company>("SELECT * FROM companies WHERE Id = @Id", new { Id = id });
        }

        public async Task CreateCompanyAsync(Company company)
        {
            var connection = await _connectionProvider.Connection();
            await connection.ExecuteAsync(
                @"INSERT INTO `companies`(`RefId`, `Address`, `City`, `Country`, `Website`, `CompanyDesc`)
                    VALUES (@RefId, @Address, @City, @Country, @Website, @CompanyDesc)",
                new
                {
                    RefId = company.RefId,
                    Address = company.Address,
                    City = company.City,
                    Country = company.Country,
                    Website = company.Website,
                    CompanyDesc = company.CompanyDesc
                });
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var connection = await _connectionProvider.Connection();
            await connection.ExecuteAsync(
                @"UPDATE `companies` 
                SET `Address` = '@Address', `City` = '@City',
                `Country` = '@Country', `Website` = '@Website', `CompanyDesc` = '@CompanyDesc'
                WHERE `RefId` = @RefId",
                new
                {
                    RefId = company.RefId,
                    Address = company.Address,
                    City = company.City,
                    Country = company.Country,
                    Website = company.Website,
                    CompanyDesc = company.CompanyDesc
                });
        }
    }
}
