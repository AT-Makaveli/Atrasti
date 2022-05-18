using System.Threading.Tasks;
using Atrasti.Data.Models;

namespace Atrasti.Data.Core
{
    public interface IShorteningRepository
    {
        Task<string> GenerateShortening(string original);
        
        Task<Shortening> GetOriginal(string shortening);
    }
}