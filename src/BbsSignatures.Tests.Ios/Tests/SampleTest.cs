using System;
using NUnit.Framework;

namespace BbsSignatures.Tests.Ios.Tests
{
    public class SampleTest
    {
        [Test]
        public void TestKey()
        {
            var key = BbsProvider.GenerateKey();

            Assert.NotNull(key);
        }
    }
}
