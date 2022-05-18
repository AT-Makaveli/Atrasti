using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    internal abstract class BaseRepository
    {
        private readonly ConnectionProvider _connectionFactory;

        protected BaseRepository(ConnectionProvider connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected async Task<T> WithConnection<T>(Func<DbConnection, Task<T>> getData, CancellationToken cancellationToken = default)
        {
            try
            {
                using (DbConnection connection = _connectionFactory.ProvideConnection())
                {
                    await connection.OpenAsync(cancellationToken);
                    return await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }

        protected async Task WithConnection(Func<DbConnection, Task> getData, CancellationToken cancellationToken = default)
        {
            try
            {
                using (DbConnection connection = _connectionFactory.ProvideConnection())
                {
                    await connection.OpenAsync(cancellationToken);
                    await getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
        }
    }
}
