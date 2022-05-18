using System;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;

namespace Atrasti.Data.Repository
{
    internal class ShorteningRepository : BaseRepository, IShorteningRepository
    {
        public ShorteningRepository(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<string> GenerateShortening(string original)
        {
            string shortening = Guid.NewGuid().ToString();

            await WithConnection(async connection =>
            {
                await connection.Insert("INSERT INTO shortenings(shortening, original) VALUES(@0, @1)", shortening,
                    original);
            });

            return shortening;
        }

        public Task<Shortening> GetOriginal(string shortening)
        {
            return WithConnection(connection => connection.SelectSingleAsync<Shortening>(
                "SELECT * FROM shortenings WHERE shortening = @0 LIMIT 1", shortening));
        }
    }
}