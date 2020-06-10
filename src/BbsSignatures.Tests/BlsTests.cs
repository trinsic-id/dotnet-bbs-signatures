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

        [Fact(DisplayName = "Generate new BLS key without seed")]
        public void GenerateKeyNoSeed()
        {
            var result = Bls.NativeMethods.bls_generate_key(ByteBuffer.None, out var publicKey, out var secretKey, out var error);

            var pubKey = publicKey.Dereference();
            var secKey = secretKey.Dereference();

            Assert.Equal(0, result);
            Assert.Equal(0, error.Code);
            Assert.NotNull(pubKey);
            Assert.NotNull(secKey);

            Assert.Equal(96, pubKey.Length);
            Assert.Equal(32, secKey.Length);
        }

        [Fact(DisplayName = "Generate new BLS key pair with seed")]
        public void GenerateKeyWithSeed()
        {
            var seed = new byte[] { 1, 2, 3 };

            var result = NativeMethods.bls_generate_key(seed, out var publicKey, out var secretKey, out var error);

            var pubKey = publicKey.Dereference();
            var secKey = secretKey.Dereference();

            Assert.Equal(0, result);
            Assert.NotNull(pubKey);
            Assert.NotNull(secKey);

            Assert.Equal(96, pubKey.Length);
            Assert.Equal(32, secKey.Length);
        }

        [Fact(DisplayName = "Generate BLS key pair without seed using wrapper class")]
        public void GenerateBlsKeyWithoutSeed()
        {
            var result = BlsSecretKey.Generate();

            Assert.NotNull(result);
            Assert.NotNull(result.GetDeterministicPublicKey());
            Assert.NotNull(result.Key);

            Assert.Equal(96, result.GetDeterministicPublicKey().Key.Count);
            Assert.Equal(32, result.Key.Count);
        }

        [Fact(DisplayName = "Create BBS public key from BLS secret key with message count 1")]
        public void CreateBbsKeyFromBlsSecretKey()
        {
            var result = BlsSecretKey.Generate();

            NativeMethods.bls_secret_key_to_bbs_key(result.Key, 1, out var publicKey, out var error);

            Assert.Equal(0, error.Code);

            var actual = publicKey.Dereference();

            Assert.NotNull(actual);
            Assert.Equal(196, actual.Length);
        }

        [Fact(DisplayName = "Create BBS public key from BLS public key with message count 1")]
        public void CreateBbsKeyFromBlsPublicKey()
        {
            var result = BlsSecretKey.Generate();
            var dpk = result.GetDeterministicPublicKey();

            NativeMethods.bls_public_key_to_bbs_key(dpk.Key, 1, out var publicKey, out var error);

            Assert.Equal(0, error.Code);

            var actual = publicKey.Dereference();

            Assert.NotNull(actual);
            Assert.Equal(196, actual.Length);
        }

        [Fact(DisplayName = "Get BLS public key from secret key")]
        public void GetPublicKeyFromSecretKey()
        {
            var result = BlsSecretKey.Generate("test");

            NativeMethods.bls_get_public_key(result.Key, out var publicKey, out var error);

            Assert.Equal(0, error.Code);
            var actual = publicKey.Dereference();
            var dpk = result.GetDeterministicPublicKey();

            Assert.NotNull(actual);
            Assert.Equal(96, actual.Length);
            Assert.True(actual.SequenceEqual(dpk.Key));
        }
    }
}
