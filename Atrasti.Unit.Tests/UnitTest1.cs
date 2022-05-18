using System;
using NUnit.Framework;

namespace Atrasti.Unit.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Aye");
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}