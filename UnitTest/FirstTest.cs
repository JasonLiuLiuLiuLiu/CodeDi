using System;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class FirstTest
    {
        [Test]
        public void Test ()
        {
           Assert.AreEqual(1,2);
        }
    }
}