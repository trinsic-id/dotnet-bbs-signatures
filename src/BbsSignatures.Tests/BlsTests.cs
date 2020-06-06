using BbsSignatures.Bls;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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

        [Fact]
        public void GenerateBlsKeyWithoutSeed()
        {
            var result = BlsKey.Create();

            Assert.NotNull(result);
            Assert.NotNull(result.PublicKey);
            Assert.NotNull(result.SecretKey);

            Assert.Equal(96, result.PublicKey.Length);
            Assert.Equal(32, result.SecretKey.Length);
        }

        [Fact]
        public void CreateBbsKeyFromBlsSecretKey()
        {
            var result = BlsKey.Create();

            NativeMethods.bls_secret_key_to_bbs_key(ByteArray.Create(result.SecretKey), 1, out var publicKey, out var error);

            Assert.Equal(0, error.Code);

            var actual = publicKey.ToByteArray();

            Assert.NotNull(actual);
        }

        [Fact]
        public void CreateBbsKeyFromBlsPublicKey()
        {
            var result = BlsKey.Create();

            NativeMethods.bls_public_key_to_bbs_key(result.PublicKey, 1, out var publicKey, out var error);

            Assert.Equal(0, error.Code);

            var ex = error.ToException();

            var actual = publicKey.ToByteArray();

            Assert.NotNull(actual);
        }

        [Fact]
        public void GetPublicKeyFromSecretKey()
        {
            var result = BlsKey.Create("test");

            NativeMethods.bls_get_public_key(result.SecretKey, out var publicKey, out var error);

            Assert.Equal(0, error.Code);
            var actual = publicKey.ToByteArray();

            Assert.NotNull(actual);
            Assert.True(actual.SequenceEqual(result.PublicKey));
        }
    }
}
