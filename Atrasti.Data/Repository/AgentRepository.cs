using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;

namespace Atrasti.Data.Repository
{
    internal class AgentRepository : BaseRepository, IAgentRepository
    {
        public AgentRepository(ConnectionProvider connectionFactory)
            : base(connectionFactory)
        {
        }

        public Task<Agent> FindAgentProfileByIdAsync(int refId)
        {
            const string query = "SELECT * FROM agent WHERE ref_id = @0 LIMIT 1";
            return WithConnection(connection => connection.SelectSingleAsync<Agent>(query, refId));
        }

        public Task CreateAgentAsync(Agent agent)
        {
            const string query = @"
INSERT INTO agent (ref_id, address, phone_number, country, county, city, website, description, b_sector, main_products, main_markets, years_experience, certificates) 
VALUES(@ref_id, @address, @phone_number, @country, @county, @city, @website, @description, @b_sector, @main_products, @main_markets, @years_experience, @certificates)";
            return WithConnection(connection => connection.ExecuteAsync(
                query,
                new
                {
                    ref_id = agent.RefId,
                    address = agent.Address,
                    phone_number = agent.PhoneNumber,
                    country = agent.Country,
                    county = agent.County,
                    city = agent.City,
                    website = agent.Website,
                    description = agent.Description,
                    b_sector = agent.BusinessSector,
                    main_products = agent.MainProducts,
                    main_markets = agent.MainMarkets,
                    years_experience = agent.YearsOfExperience,
                    certificates = agent.Certificates,
                }));
        }

        public Task UpdateAgentAsync(Agent agent)
        {
            const string query = @"
UPDATE agent SET address = @address, phone_number = @phone_number, country = @country, 
                 county = @county, city = @city, website = @website, description = @description,
                 b_sector = @b_sector, main_products = @main_products, main_markets = @main_markets, 
                 years_experience = @years_experience, certificates = @certificates WHERE ref_id = @ref_id";
            return WithConnection(connection => connection.ExecuteAsync(query, new
            {
                ref_id = agent.RefId,
                address = agent.Address,
                phone_number = agent.PhoneNumber,
                country = agent.Country,
                county = agent.County,
                city = agent.City,
                website = agent.Website,
                description = agent.Description,
                b_sector = agent.BusinessSector,
                main_products = agent.MainProducts,
                main_markets = agent.MainMarkets,
                years_experience = agent.YearsOfExperience,
                certificates = agent.Certificates,
            }));
        }
    }
}