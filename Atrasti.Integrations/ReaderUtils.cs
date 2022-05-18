using System;
using System.Data;

namespace Atrasti.Integrations
{
    public static class ReaderUtils
    {
        public static T GetData<T>(this IDataReader reader, string column) =>
            (T) reader[column];

        public static T GetDataNullable<T>(this IDataReader reader, string column)
        {
            if (reader[column] != DBNull.Value) 
                return (T) reader[column];

            return default;
        }
    }
}