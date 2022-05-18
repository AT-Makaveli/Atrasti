using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Atrasti.Data
{
    internal static class QueryBuilder
    {
        internal static T GetData<T>(this IDataReader dataReader, string column) =>
            (T) dataReader[column];

        internal static T? GetDataNullable<T>(this IDataReader dataReader, string column) where T : struct
        {
            object columnValue = dataReader[column];

            if (!(columnValue is DBNull))
                return (T) columnValue;

            return null;
        }

        internal static string GetDataNullable(this IDataReader dataReader, string column)
        {
            object columnValue = dataReader[column];

            if (!(columnValue is DBNull))
                return (string) columnValue;

            return null;
        }

        internal static DateTime GetDate(this IDataReader dataReader, string column) =>
            Convert.ToDateTime(dataReader[column]);

        internal static bool HasColumn(this IDataReader dataReader, string columnName)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (dataReader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        internal static async Task<TSerializable> SelectSingleAsync<TSerializable>(this DbConnection connection,
            string query, params object[] parameters)
            where TSerializable : ISerializable, new()
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);
            using DbDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                TSerializable value = new TSerializable();
                value.Serialize(reader);
                return value;
            }

            return default;
        }

        internal static async Task<IList<TSerializable>> SelectMultipleAsync<TSerializable>(
            this DbConnection connection, string query, params object[] parameters)
            where TSerializable : ISerializable, new()
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);
            return await command.SelectMultipleAsync<TSerializable>();
        }

        internal static async Task<IList<TSerializable>> SelectMultipleAsync<TSerializable>(this DbCommand command)
            where TSerializable : ISerializable, new()
        {
            using DbDataReader reader = await command.ExecuteReaderAsync();

            IList<TSerializable> returnList = new List<TSerializable>();
            while (await reader.ReadAsync())
            {
                TSerializable value = new TSerializable();
                value.Serialize(reader);
                returnList.Add(value);
            }

            return returnList;
        }

        internal static DbCommand CreateCommandWithIntArray(this DbConnection connection, string query, int[] array)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            for (int i = 0; i < array.Length; i++)
            {
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = i.ToString();
                param.Value = array[i];
                command.Parameters.Add(param);
            }

            return command;
        }

        internal static DbCommand CreateCommandWithStringArray(this DbConnection connection, string query,
            string[] array, bool wildCardRight = false)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            for (int i = 0; i < array.Length; i++)
            {
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = i.ToString();
                string value = array[i];
                if (wildCardRight) value += "%";
                param.Value = value;
                command.Parameters.Add(param);
            }

            return command;
        }

        internal static Task<int> Insert(this DbConnection connection, string query, params object[] parameters)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);

            return command.ExecuteNonQueryAsync();
        }

        internal static Task<object> InsertScalar(this DbConnection connection, string query,
            params object[] parameters)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;
            AddParameters(command, parameters);

            return command.ExecuteScalarAsync();
        }


        private static void AddParameters(this IDbCommand command, object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = i.ToString();
                param.Value = parameters[i];
                command.Parameters.Add(param);
            }
        }
    }
}