using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsIntegrationTests
    {
        [Fact(DisplayName = "Full end-to-end test")]
        public async Task FullDemoTest()
        {
            var key = BbsProvider.Create();
            var publicKey = key.GeneratePublicKey(3);

            var nonce = "123";
            var messages = new[]
            {
                "message_1",
                "message_2",
                "message_3"
            };

            // Sign messages
            var signature = await BbsProvider.SignAsync(key, publicKey, messages);

            signature.Should().NotBeNull().And.HaveCount(BbsProvider.SignatureSize);

            // Verify messages

            var verifySignatureResult = await BbsProvider.VerifyAsync(publicKey, messages, signature);

            verifySignatureResult.Should().BeTrue();

            // Create blind commitment
            var blindedMessages = new[]
            {
                new IndexedMessage { Index = 0, Message = messages[0] }
            };
            var commitment = await BbsProvider.CreateBlindCommitmentAsync(publicKey, nonce, blindedMessages);

            Assert.NotNull(commitment);

            // Verify blinded commitment
            var verifyResult = await BbsProvider.VerifyBlindedCommitmentAsync(commitment.BlindSignContext.ToArray(), new [] { 0u }, publicKey, nonce);
            
            Assert.Equal(SignatureProofStatus.Success, verifyResult);

            // Blind sign
            var messagesToSign = new[]
            {
                new IndexedMessage { Index = 1, Message = messages[1] },
                new IndexedMessage { Index = 2, Message = messages[2] }
            };
            var blindedSignature = await BbsProvider.BlindSignAsync(key, publicKey, commitment.Commitment.ToArray(), messagesToSign);

            blindedSignature.Should().NotBeNull().And.HaveCount(BbsProvider.BlindSignatureSize);

            // Unblind signature
            var unblindedSignature = await BbsProvider.UnblindSignatureAsync(blindedSignature, commitment.BlindingFactor.ToArray());

            unblindedSignature.Should().NotBeNull().And.HaveCount(BbsProvider.SignatureSize);

            // Verify signature
            var verifyUnblindedSignatureResult = await BbsProvider.VerifyAsync(publicKey, messages.ToArray(), unblindedSignature);

            verifyUnblindedSignatureResult.Should().BeTrue();

            // Create proof
            var proofMessages = new[]
            {
                new ProofMessage { Message = messages[0], ProofType = ProofMessageType.Revealed },
                new ProofMessage { Message = messages[1], ProofType = ProofMessageType.HiddenProofSpecificBlinding },
                new ProofMessage { Message = messages[2], ProofType = ProofMessageType.Revealed }
            };

            var proof = await BbsProvider.CreateProofAsync(publicKey, proofMessages, commitment.BlindingFactor.ToArray(), unblindedSignature, nonce);

            Assert.NotNull(proof);

            // Verify Proof
            var indexedMessages = new[]
            {
                new IndexedMessage { Message = messages[0], Index = 0u },
                new IndexedMessage { Message = messages[2], Index = 2u }
            };

            var verifyProofResult = await BbsProvider.VerifyProofAsync(publicKey, proof, indexedMessages, nonce);

            Assert.Equal(SignatureProofStatus.Success, verifyProofResult);
        }
    }
}
