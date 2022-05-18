using System.Threading.Tasks;

namespace Atrasti.Integrations
{
    public interface IIntegration
    {
        Task IntegrateAsync();
    }
}