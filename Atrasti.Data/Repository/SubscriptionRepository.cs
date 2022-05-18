using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Repository
{
    internal class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        private static readonly IDictionary<string, Subscription> _subscriptions =
            new Dictionary<string, Subscription>();

        private static int access = 0;
        private readonly IUserRepository _userRepository;

        public SubscriptionRepository(ConnectionProvider connectionProvider, IUserRepository userRepository)
            : base(connectionProvider)
        {
            _userRepository = userRepository;
        }

        public async Task<IDictionary<string, Subscription>> FetchSubscriptions()
        {
            if (_subscriptions.Count > 0 && access < 10)
            {
                access++;
                return _subscriptions;
            }

            access = 0;

            return await WithConnection(async connection =>
            {
                IEnumerable<Subscription> subscriptions =
                    await connection.QueryAsync<Subscription>("SELECT * FROM subscriptions;");
                foreach (Subscription subscription in subscriptions)
                {
                    subscription.User = await _userRepository.FindSetupUserById(subscription.CompanyId);
                }

                return subscriptions.ToDictionary(x => x.PlaceHolder, x => x);
            }, CancellationToken.None);
        }
    }
}