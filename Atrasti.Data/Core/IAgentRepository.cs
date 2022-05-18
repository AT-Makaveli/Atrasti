using System.Threading.Tasks;
using Atrasti.Data.Models;

namespace Atrasti.Data.Core
{
    public interface IAgentRepository
    {
        Task<Agent> FindAgentProfileByIdAsync(int refId);

        Task CreateAgentAsync(Agent agent);
        
        Task UpdateAgentAsync(Agent agent);
    }
}