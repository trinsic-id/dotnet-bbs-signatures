using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsIntegrationTests
    {
        [Fact(DisplayName = "Full end-to-end test")]
        public void FullDemoTest()
        {
            var key = BbsProvider.GenerateKey();
            var publicKey = key.GeneratePublicKey(3);

            var nonce = "123";
            var messages = new[]
            {
                "message_1",
                "message_2",
                "message_3"
            };

            // Sign messages
            var signature = BbsProvider.Sign(key, publicKey, messages);

            signature.Should().NotBeNull().And.HaveCount(BbsProvider.SignatureSize);

            // Verify messages

            var verifySignatureResult = BbsProvider.Verify(publicKey, messages, signature);

            verifySignatureResult.Should().BeTrue();

            // Create blind commitment
            var blindedMessages = new[]
            {
                new IndexedMessage { Index = 0, Message = messages[0] }
            };
            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, nonce, blindedMessages);

            Assert.NotNull(commitment);

            // Verify blinded commitment
            var verifyResult = BbsProvider.VerifyBlindedCommitment(commitment.BlindSignContext.ToArray(), new [] { 0u }, publicKey, nonce);

            verifyResult.Should().Be(SignatureProofStatus.Success);

            // Blind sign
            var messagesToSign = new[]
            {
                new IndexedMessage { Index = 1, Message = messages[1] },
                new IndexedMessage { Index = 2, Message = messages[2] }
            };
            var blindedSignature = BbsProvider.BlindSign(key, publicKey, commitment.Commitment.ToArray(), messagesToSign);

            blindedSignature.Should().NotBeNull().And.HaveCount(BbsProvider.BlindSignatureSize);

            // Unblind signature
            var unblindedSignature = BbsProvider.UnblindSignature(blindedSignature, commitment.BlindingFactor.ToArray());

            unblindedSignature.Should().NotBeNull().And.HaveCount(BbsProvider.SignatureSize);

            // Verify signature
            var verifyUnblindedSignatureResult = BbsProvider.Verify(publicKey, messages.ToArray(), unblindedSignature);

            verifyUnblindedSignatureResult.Should().BeTrue();

            // Create proof
            var proofMessages = new[]
            {
                new ProofMessage { Message = messages[0], ProofType = ProofMessageType.Revealed },
                new ProofMessage { Message = messages[1], ProofType = ProofMessageType.HiddenProofSpecificBlinding },
                new ProofMessage { Message = messages[2], ProofType = ProofMessageType.Revealed }
            };

            var proof = BbsProvider.CreateProof(publicKey, proofMessages, commitment.BlindingFactor.ToArray(), unblindedSignature, nonce);

            proof.Should().NotBeNull().And.NotBeEmpty();

            // Verify proof
            var indexedMessages = new[]
            {
                new IndexedMessage { Message = messages[0], Index = 0u },
                new IndexedMessage { Message = messages[2], Index = 2u }
            };

            var verifyProofResult = BbsProvider.VerifyProof(publicKey, proof, indexedMessages, nonce);

            verifyProofResult.Should().Be(SignatureProofStatus.Success);
        }
    }
}
