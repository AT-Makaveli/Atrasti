using System;

namespace Atrasti.Utils
{
    public static class CollectionUtils
    {
        public static string SelectRandom(params string[] strings)
        {
            Random random = new Random();
            int index = random.Next(strings.Length);

            return strings[index];
        }
    }
}