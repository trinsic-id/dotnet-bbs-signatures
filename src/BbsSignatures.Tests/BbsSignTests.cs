using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsSignTests
    {
        [Fact(DisplayName = "Get signature size")]
        public void GetSignatureSize()
        {
            var result = NativeMethods.bbs_signature_size();

            Assert.Equal(112, result);
        }

        [Fact(DisplayName = "Sign message")]
        public async Task SignSingleMessageUsingApi()
        {
            var myKey = BbsProvider.Create();
            var publiKey = myKey.GeneratePublicKey(1);

            var signature = await BbsProvider.SignAsync(myKey, publiKey, new[] { "message" });

            Assert.NotNull(signature);
            Assert.Equal(signature.Length, NativeMethods.bbs_signature_size());
        }

        [Fact(DisplayName = "Sign multiple messages")]
        public async Task SignMultipleeMessages()
        {
            var keyPair = BbsProvider.Create();
            var publicKey = keyPair.GeneratePublicKey(2);

            var signature = await BbsProvider.SignAsync(keyPair, publicKey, new[] { "message_1", "message_2" });

            signature.Should().NotBeNull().And.HaveCount(BbsProvider.SignatureSize);
        }

        [Fact(DisplayName = "Verify throws if invalid signature")]
        public async Task VerifyThrowsIfInvalidSignature()
        {
            var secretKey = BbsProvider.Create();
            var publicKey = secretKey.GeneratePublicKey(1);

            Func<Task> verifySignature = () => BbsProvider.VerifyAsync(publicKey, new[] { "message_0" }, Array.Empty<byte>());

            await verifySignature.Should().ThrowAsync<BbsException>("Signature cannot be empty array");
        }
    }
}
