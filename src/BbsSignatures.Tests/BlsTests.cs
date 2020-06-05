using BbsSignatures.Bls;
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

        [Fact]
        public void GenerateKeyNoSeed()
        {
            var result = Bls.NativeMethods.bls_generate_key(ByteArray.None, out var publicKey, out var secretKey, out var error);

            var pubKey = publicKey.ToByteArray();
            var secKey = secretKey.ToByteArray();

            Assert.Equal(0, result);
            Assert.Equal(0, error.Code);
            Assert.NotNull(pubKey);
            Assert.NotNull(secKey);

            Assert.Equal(96, pubKey.Length);
            Assert.Equal(32, secKey.Length);
        }

        [Fact]
        public void GenerateKeyWithSeed()
        {
            var seed = new byte[] { 1, 2, 3 };

            var result = Bls.NativeMethods.bls_generate_key(ByteArray.Create(seed), out var publicKey, out var secretKey, out var error);

            var pubKey = publicKey.ToByteArray();
            var secKey = secretKey.ToByteArray();

            Assert.Equal(0, result);
            Assert.NotNull(pubKey);
            Assert.NotNull(secKey);

            Assert.Equal(96, pubKey.Length);
            Assert.Equal(32, secKey.Length);
        }
    }
}
