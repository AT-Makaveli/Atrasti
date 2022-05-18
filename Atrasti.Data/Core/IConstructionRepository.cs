using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface IConstructionRepository
    {
        Task<bool> SignUpEmailForNotificationAsync(string email, string continent);
    }
}