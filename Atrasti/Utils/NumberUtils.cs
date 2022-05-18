using System;

namespace Atrasti.Utils
{
    public static class NumberUtils
    {
        public static int CombineIntToHash(this int x, int y)
        {
            if (y > x) return y.CombineIntToHash(x);

            int result = 1;

            result = 31 + x;
            result = 31 * result + y;

            return result;
        }

        public static int UnixTimeStamp()
        {
            return (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}