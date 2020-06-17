using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BbsSignatures.Tests
{
    public class BlsTests
    {
        [Test(Description = "Get BLS secret key size")]
        public void GetSecretKeySize()
        {
            var actual = NativeMethods.bls_secret_key_size();

            Assert.AreEqual(actual, 32);
        }

        [Test(Description = "Get BLS public key size")]
        public void GetPublicKeySize()
        {
            var actual = NativeMethods.bls_public_key_size();

            Assert.AreEqual(actual, 96);
        }

        [Test(Description = "Generate new BLS key pair with seed")]
        public void GenerateKeyWithSeed()
        {
            var seed = new byte[] { 1, 2, 3 };

            var actual = BbsProvider.GenerateKey(seed);

            Assert.NotNull(actual);
            Assert.NotNull(actual.SecretKey);
            Assert.NotNull(actual.PublicKey);
            Assert.AreEqual(32, actual.SecretKey.Count);
            Assert.AreEqual(96, actual.PublicKey.Count);
        }

        [Test(Description = "Generate BLS key pair without seed using wrapper class")]
        public void GenerateBlsKeyWithoutSeed()
        {
            var blsKeyPair = BbsProvider.GenerateKey();
            var dPublicKey = blsKeyPair.PublicKey;

            Assert.NotNull(blsKeyPair);
            Assert.NotNull(dPublicKey);
            Assert.NotNull(blsKeyPair.SecretKey);

            Assert.AreEqual(96, dPublicKey.Count);
            Assert.AreEqual(32, blsKeyPair.SecretKey.Count);
        }

        [Test(Description = "Create BBS public key from BLS secret key with message count 1")]
        public void CreateBbsKeyFromBlsSecretKey()
        {
            var secretKey = BbsProvider.GenerateKey();
            var publicKey = secretKey.GeneratePublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.SecretKey);

            Assert.AreEqual(196, publicKey.Count);
            Assert.AreEqual(32, secretKey.SecretKey.Count);
        }

        [Test(Description = "Create BBS public key from BLS public key with message count 1")]
        public void CreateBbsKeyFromBlsPublicKey()
        {
            var secretKey = BbsProvider.GenerateKey();
            var dPublicKey = secretKey.PublicKey;
            var publicKey = dPublicKey.CreateBbsPublicKey(1);

            Assert.NotNull(secretKey);
            Assert.NotNull(publicKey);
            Assert.NotNull(secretKey.SecretKey);

            Assert.AreEqual(196, publicKey.Count);
            Assert.AreEqual(32, secretKey.SecretKey.Count);
        }
    }
}
