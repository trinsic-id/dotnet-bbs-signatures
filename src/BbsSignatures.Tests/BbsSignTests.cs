using System;
using NUnit.Framework;

namespace BbsSignatures.Tests
{
    public class BbsSignTests
    {
        [Test(Description = "Get signature size")]
        public void GetSignatureSize()
        {
            var result = NativeMethods.bbs_signature_size();

            Assert.AreEqual(112, result);
        }

        [Test(Description = "Sign message")]
        public void SignSingleMessageUsingApi()
        {
            var myKey = BbsProvider.GenerateKey();
            var publiKey = myKey.GeneratePublicKey(1);

            var signature = BbsProvider.Sign(myKey, publiKey, new[] { "message" });

            Assert.NotNull(signature);
            Assert.AreEqual(signature.Length, NativeMethods.bbs_signature_size());
        }

        [Test(Description = "Sign multiple messages")]
        public void SignMultipleeMessages()
        {
            var keyPair = BbsProvider.GenerateKey();
            var publicKey = keyPair.GeneratePublicKey(2);

            var signature = BbsProvider.Sign(keyPair, publicKey, new[] { "message_1", "message_2" });

            Assert.NotNull(signature);
            Assert.AreEqual(BbsProvider.SignatureSize, signature.Length);
        }

        [Test(Description = "Verify throws if invalid signature")]
        public void VerifyThrowsIfInvalidSignature()
        {
            var secretKey = BbsProvider.GenerateKey();
            var publicKey = secretKey.GeneratePublicKey(1);

            Assert.Throws<BbsException>(() => BbsProvider.Verify(publicKey, new[] { "message_0" }, Array.Empty<byte>()), "Signature cannot be empty array");
        }
    }
}
