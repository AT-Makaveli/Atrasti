using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface ISubscriptionRepository
    {
        Task<IDictionary<string, Subscription>> FetchSubscriptions(); 
    }
}
