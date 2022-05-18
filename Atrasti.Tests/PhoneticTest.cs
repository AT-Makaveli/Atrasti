using System;
using Phonix;

namespace Atrasti.Tests
{
    public class PhoneticTest
    {
        public static void Test()
        {
            DoubleMetaphone doubleMetaphone = new DoubleMetaphone();
            Console.WriteLine(doubleMetaphone.BuildKey("blue red"));
            Console.WriteLine(doubleMetaphone.BuildKey("bloo röd"));
        }
    }
}