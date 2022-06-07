using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Repository
{
    internal class CompanyRepository : BaseRepository, ICompanyRepository
    {
        private readonly IProductRepository _productRepository;

        public CompanyRepository(ConnectionProvider connectionFactory, IProductRepository productRepository) : base(
            connectionFactory)
        {
            _productRepository = productRepository;
        }

        public Task CreateCompanyAsync(Company company)
        {
            return WithConnection(async connection =>
            {
                using IDbTransaction transaction = await connection.BeginTransactionAsync(CancellationToken.None);
                await connection.ExecuteAsync(
                    "INSERT INTO `companies`(`RefId`, `Address`, `City`, `State`, `Country`, `Website`, `CompanyDesc`) VALUES (@refId, @address, @city, @state, @country, @website, @desc);",
                    new
                    {
                        refId = company.RefId,
                        address = company.Address,
                        state = company.State,
                        city = company.City,
                        country = company.Country,
                        website = company.Website,
                        desc = company.CompanyDesc
                    }, transaction);

                await connection.ExecuteAsync(
                    "INSERT INTO `company_infos`(`RefId`, `BusinessType`, `MainProducts`, `MainMarkets`, `Certificates`, `YearEstablished`, `Capacity`) VALUES (@refId, @busType, @mainProd, @mainMark, @cert, @yearEst, @capacity);",
                    new
                    {
                        refId = company.RefId,
                        busType = company.CompanyInfo.BusinessType,
                        mainProd = company.CompanyInfo.MainProducts,
                        mainMark = company.CompanyInfo.MainMarkets,
                        cert = company.CompanyInfo.Certificates,
                        yearEst = company.CompanyInfo.YearEstablished,
                        capacity = company.CompanyInfo.Capacity
                    }, transaction);
                transaction.Commit();
            }, CancellationToken.None);
        }

        public Task<Company> FindCompanyByIdAsync(int id)
        {
            return WithConnection(async connection =>
            {
                using IDbTransaction transaction = await connection.BeginTransactionAsync(CancellationToken.None);
                Company company = await connection.QuerySingleOrDefaultAsync<Company>(
                    "SELECT companies.*, Users.Company as CompanyName FROM companies JOIN Users ON companies.RefId = Users.Id WHERE companies.RefId = @id",
                    new {id}, transaction);
                if (company != null)
                    company.CompanyInfo = await connection.QuerySingleOrDefaultAsync<CompanyInfo>(
                        "SELECT * FROM company_infos WHERE RefId = @refId", new {refId = id}, transaction);

                transaction.Commit();
                return company;
            }, CancellationToken.None);
        }

        public Task UpdateCompanyAsync(Company company)
        {
            return WithConnection(async connection =>
            {
                using IDbTransaction transaction = await connection.BeginTransactionAsync(CancellationToken.None);
                await connection.ExecuteAsync(
                    "UPDATE `companies` SET `Address` = @address, `City` = @city, `State` = @state, `Country` = @country, `Website` = @website, `CompanyDesc` = @companyDesc WHERE `RefId` = @refId LIMIT 1;",
                    new
                    {
                        refId = company.RefId,
                        address = company.Address,
                        city = company.City,
                        state = company.State,
                        country = company.Country,
                        website = company.Website,
                        companyDesc = company.CompanyDesc
                    }, transaction);
                await connection.ExecuteAsync(
                    "UPDATE `company_infos` SET `BusinessType` = @busType, `MainProducts` = @mainProd, `MainMarkets` = @mainMark, `Certificates` = @cert, `YearEstablished` = @yearEst, `Capacity` = @capacity WHERE `RefId` = @refId LIMIT 1;",
                    new
                    {
                        refId = company.RefId,
                        busType = company.CompanyInfo.BusinessType,
                        mainProd = company.CompanyInfo.MainProducts,
                        mainMark = company.CompanyInfo.MainMarkets,
                        cert = company.CompanyInfo.Certificates,
                        yearEst = company.CompanyInfo.YearEstablished,
                        capacity = company.CompanyInfo.Capacity
                    }, transaction);
                transaction.Commit();
            }, CancellationToken.None);
        }

        public Task<ICollection<Company>> Get20ProductsAsync()
        {
            return WithConnection(async connection =>
            {
                ICollection<Company> companies = new List<Company>();
                using IDataReader reader =
                    await connection.ExecuteReaderAsync("SELECT DISTINCT CompanyId FROM products LIMIT 20;");
                while (reader.Read())
                {
                    int companyId = reader.GetData<int>("CompanyId");
                    Company company = await FindCompanyByIdAsync(companyId);
                    company.Products = await _productRepository.FindProductsByCompanyIdAsync(companyId);
                    companies.Add(company);
                }

                return companies;
            }, CancellationToken.None);
        }
    }
}