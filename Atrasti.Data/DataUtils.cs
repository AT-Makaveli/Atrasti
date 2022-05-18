using System;
using System.Collections.Generic;

namespace Atrasti.Data
{
    public static class DataUtils
    {
        public static void ThrowIfNull<T>(this T type, string name)
        {
            if (type == null)
            {
                throw new NullReferenceException($"Parameter: {name} can't be null.");
            }
        }

        public static T[] ToArray<T>(this IList<T> value)
        {
            T[] array = new T[value.Count];
            value.CopyTo(array, 0);

            return array;
        }
    }
}
