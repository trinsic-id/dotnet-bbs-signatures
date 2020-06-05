using System;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BlsTests
    {
        [Fact]
        public void GetSecretKeySize()
        {
            var result = Bls.NativeMethods.bls_secret_key_size();

            Assert.Equal(32, result);
        }

        [Fact]
        public void GetPublicKeySize()
        {
            var result = Bls.NativeMethods.bls_public_key_size();

            Assert.Equal(96, result);
        }
    }
}
