using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Atrasti.Data
{
    internal class ConnectionProvider
    {
        private readonly string _connectionString;

        public ConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        internal async Task<IDbConnection> Connection()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        internal DbConnection ProvideConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
