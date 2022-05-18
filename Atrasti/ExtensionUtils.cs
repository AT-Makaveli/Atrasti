using System;

namespace Atrasti
{
    internal static class ExtensionUtils
    {
        internal static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (T item in array)
            {
                action(item);
            }
        }
    }
}
