using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BlsTests
    {
        [Fact(DisplayName = "Get BLS secret key size")]
        public void GetSecretKeySize()
        {
            var actual = NativeMethods.bls_secret_key_size();

            actual.Should().Be(32);
        }

        [Fact(DisplayName = "Get BLS public key size")]
        public void GetPublicKeySize()
        {
            var actual = NativeMethods.bls_public_key_size();

            actual.Should().Be(96);
        }

        [Fact(DisplayName = "Generate new BLS key pair with seed")]
        public void GenerateKeyWithSeed()
        {
            var seed = new byte[] { 1, 2, 3 };

            var actual = BbsProvider.GenerateKey(seed);

            actual.Should().NotBeNull();
            actual.SecretKey.Should().NotBeNull().And.HaveCount(32);
            actual.PublicKey.Should().NotBeNull().And.HaveCount(96);
        }

        [Fact(DisplayName = "Generate BLS key pair without seed using wrapper class")]
        public void GenerateBlsKeyWithoutSeed()
        {
            var blsKeyPair = BbsProvider.GenerateKey();
            var dPublicKey = blsKeyPair.PublicKey;

            Assert.NotNull(blsKeyPair);
            Assert.NotNull(dPublicKey);
            Assert.NotNull(blsKeyPair.SecretKey);

            Assert.Equal(96, dPublicKey.Count);
            Assert.Equal(32, blsKeyPair.SecretKey.Count);
        }

        [Fact(DisplayName = "Create BBS public key from BLS secret key with message count 1")]
        public void CreateBbsKeyFromBlsSecretKey()
        {
            var secretKey = BbsProvider.GenerateKey();
            var publicKey = secretKey.GeneratePublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.SecretKey);

            Assert.Equal(196, publicKey.Count);
            Assert.Equal(32, secretKey.SecretKey.Count);
        }

        [Fact(DisplayName = "Create BBS public key from BLS public key with message count 1")]
        public void CreateBbsKeyFromBlsPublicKey()
        {
            var secretKey = BbsProvider.GenerateKey();
            var dPublicKey = secretKey.PublicKey;
            var publicKey = dPublicKey.CreateBbsPublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.SecretKey);

            Assert.Equal(196, publicKey.Count);
            Assert.Equal(32, secretKey.SecretKey.Count);
        }
    }
}
