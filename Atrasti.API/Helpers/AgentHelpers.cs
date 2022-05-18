using Atrasti.API.Models.Profile;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class AgentHelpers
    {
        public static AgentPage_Res MapAgent(this Agent agent, string logo)
        {
            return new AgentPage_Res()
            {
                Logo = logo,
                Address = agent.Address,
                PhoneNumber = agent.PhoneNumber,
                Country = agent.Country,
                County = agent.County,
                City = agent.City,
                Website = agent.Website,
                Description = agent.Description,
                BusinessSector = agent.BusinessSector,
                MainProducts = agent.MainProducts,
                MainMarkets = agent.MainMarkets,
                YearsOfExperience = agent.YearsOfExperience,
                Certificates = agent.Certificates,
            };
        }

        public static Agent MapCreateAgent(this ManageAgent_Req agent, int userId)
        {
            return new Agent()
            {
                RefId = userId,
                Address = agent.Address,
                PhoneNumber = agent.PhoneNumber,
                Country = agent.Country,
                County = agent.County,
                City = agent.City,
                Website = agent.Website,
                Description = agent.Description,
                BusinessSector = agent.BusinessSector,
                MainProducts = agent.MainProducts,
                MainMarkets = agent.MainMarkets,
                YearsOfExperience = agent.YearsOfExperience,
                Certificates = agent.Certificates,
            };
        }

        public static Agent MapUpdateAgent(this ManageAgent_Req manageAgent, Agent agent)
        {
            if (!string.IsNullOrEmpty(manageAgent.Address)) agent.Address = manageAgent.Address;
            if (!string.IsNullOrEmpty(manageAgent.PhoneNumber)) agent.PhoneNumber = manageAgent.PhoneNumber;
            if (!string.IsNullOrEmpty(manageAgent.Country)) agent.Country = manageAgent.Country;
            if (!string.IsNullOrEmpty(manageAgent.County)) agent.County = manageAgent.County;
            if (!string.IsNullOrEmpty(manageAgent.City)) agent.City = manageAgent.City;
            if (!string.IsNullOrEmpty(manageAgent.Website)) agent.Website = manageAgent.Website;
            if (!string.IsNullOrEmpty(manageAgent.Description)) agent.Description = manageAgent.Description;
            if (!string.IsNullOrEmpty(manageAgent.BusinessSector)) agent.BusinessSector = manageAgent.BusinessSector;
            if (!string.IsNullOrEmpty(manageAgent.MainProducts)) agent.MainProducts = manageAgent.MainProducts;
            if (!string.IsNullOrEmpty(manageAgent.MainMarkets)) agent.MainMarkets = manageAgent.MainMarkets;
            if (!string.IsNullOrEmpty(manageAgent.YearsOfExperience)) agent.YearsOfExperience = manageAgent.YearsOfExperience;
            if (!string.IsNullOrEmpty(manageAgent.Certificates)) agent.Certificates = manageAgent.Certificates;
            
            return agent;
        }
    }
}