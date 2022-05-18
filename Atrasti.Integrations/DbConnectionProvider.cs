using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Atrasti.Integrations
{
    public class DbConnectionProvider
    {
        private readonly string _connectionString;

        public DbConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
        }

        public async Task<DbConnection> ProvideConnection()
        {
            DbConnection connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}