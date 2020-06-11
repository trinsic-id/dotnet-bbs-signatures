using BbsSignatures.Bls;
using System;
using System.Linq;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BlsTests
    {
        [Fact(DisplayName = "Get BLS secret key size")]
        public void GetSecretKeySize()
        {
            var result = NativeMethods.bls_secret_key_size();

            Assert.Equal(32, result);
        }

        [Fact(DisplayName = "Get BLS public key size")]
        public void GetPublicKeySize()
        {
            var result = NativeMethods.bls_public_key_size();

            Assert.Equal(96, result);
        }

        [Fact(DisplayName = "Generate new BLS key pair with seed")]
        public void GenerateKeyWithSeed()
        {
            var seed = new byte[] { 1, 2, 3 };

            var secretKey = BlsSecretKey.Generate(seed);
            var dPublicKey = secretKey.GetDeterministicPublicKey();

            Assert.NotNull(secretKey);
            Assert.NotNull(dPublicKey);
            Assert.NotNull(secretKey.Key);

            Assert.Equal(96, dPublicKey.Key.Count);
            Assert.Equal(32, secretKey.Key.Count);
        }

        [Fact(DisplayName = "Generate BLS key pair without seed using wrapper class")]
        public void GenerateBlsKeyWithoutSeed()
        {
            var secretKey = BlsSecretKey.Generate();
            var dPublicKey = secretKey.GetDeterministicPublicKey();

            Assert.NotNull(secretKey);
            Assert.NotNull(dPublicKey);
            Assert.NotNull(secretKey.Key);

            Assert.Equal(96, dPublicKey.Key.Count);
            Assert.Equal(32, secretKey.Key.Count);
        }

        [Fact(DisplayName = "Create BBS public key from BLS secret key with message count 1")]
        public void CreateBbsKeyFromBlsSecretKey()
        {
            var secretKey = BlsSecretKey.Generate();
            var publicKey = secretKey.GeneratePublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.Key);

            Assert.Equal(196, publicKey.Key.Count);
            Assert.Equal(32, secretKey.Key.Count);
        }

        [Fact(DisplayName = "Create BBS public key from BLS public key with message count 1")]
        public void CreateBbsKeyFromBlsPublicKey()
        {
            var secretKey = BlsSecretKey.Generate();
            var dPublicKey = secretKey.GetDeterministicPublicKey();
            var publicKey = dPublicKey.GeneratePublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.Key);

            Assert.Equal(196, publicKey.Key.Count);
            Assert.Equal(32, secretKey.Key.Count);
        }
    }
}
