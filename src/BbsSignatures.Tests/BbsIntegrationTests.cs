using NUnit.Framework;
using System.Linq;

namespace BbsSignatures.Tests
{
    public class BbsIntegrationTests
    {
        [Test(Description = "Full end-to-end test")]
        public void FullDemoTest()
        {
            var key = BbsProvider.GenerateBlsKey();
            var publicKey = key.GenerateBbsKey(3);

            var nonce = "123";
            var messages = new[]
            {
                "message_1",
                "message_2",
                "message_3"
            };

            // Sign messages
            var signature = BbsProvider.Sign(key, messages);

            Assert.NotNull(signature);
            Assert.AreEqual(BbsProvider.SignatureSize, signature.Length);

            // Verify messages

            var verifySignatureResult = BbsProvider.Verify(publicKey, messages, signature);

            Assert.True(verifySignatureResult);

            // Create proof
            var proofMessages1 = new []
            {
                new ProofMessage { Message = messages[0], ProofType = ProofMessageType.Revealed },
                new ProofMessage { Message = messages[1], ProofType = ProofMessageType.Revealed },
                new ProofMessage { Message = messages[2], ProofType = ProofMessageType.Revealed }
            };
            var proofResult = BbsProvider.CreateProof(publicKey, proofMessages1, null, signature, nonce);

            Assert.NotNull(proofResult);
            
            // Verify proof of revealed messages
            var indexedMessages1 = new[]
            {
                new IndexedMessage { Message = messages[0], Index = 0u },
                new IndexedMessage { Message = messages[1], Index = 1u },
                new IndexedMessage { Message = messages[2], Index = 2u }
            };
            var verifyResult1 = BbsProvider.VerifyProof(publicKey, proofResult, indexedMessages1, nonce);

            Assert.AreEqual(SignatureProofStatus.Success, verifyResult1);

            // Create blind commitment
            var blindedMessages = new[]
            {
                new IndexedMessage { Index = 0, Message = messages[0] }
            };
            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, nonce, blindedMessages);

            Assert.NotNull(commitment);

            // Verify blinded commitment
            var verifyResult = BbsProvider.VerifyBlindedCommitment(commitment.BlindSignContext.ToArray(), new [] { 0u }, publicKey, nonce);

            Assert.AreEqual(SignatureProofStatus.Success, verifyResult);

            // Blind sign
            var messagesToSign = new[]
            {
                new IndexedMessage { Index = 1, Message = messages[1] },
                new IndexedMessage { Index = 2, Message = messages[2] }
            };
            var blindedSignature = BbsProvider.BlindSign(key, publicKey, commitment.Commitment.ToArray(), messagesToSign);

            Assert.NotNull(blindedSignature);
            Assert.AreEqual(BbsProvider.BlindSignatureSize, blindedSignature.Length);

            // Unblind signature
            var unblindedSignature = BbsProvider.UnblindSignature(blindedSignature, commitment.BlindingFactor.ToArray());

            Assert.NotNull(unblindedSignature);
            Assert.AreEqual(BbsProvider.SignatureSize, unblindedSignature.Length);

            // Verify signature
            var verifyUnblindedSignatureResult = BbsProvider.Verify(publicKey, messages.ToArray(), unblindedSignature);

            Assert.True(verifyUnblindedSignatureResult);

            // Create proof
            var proofMessages = new[]
            {
                new ProofMessage { Message = messages[0], ProofType = ProofMessageType.Revealed },
                new ProofMessage { Message = messages[1], ProofType = ProofMessageType.HiddenProofSpecificBlinding },
                new ProofMessage { Message = messages[2], ProofType = ProofMessageType.Revealed }
            };

            var proof = BbsProvider.CreateProof(publicKey, proofMessages, commitment.BlindingFactor.ToArray(), unblindedSignature, nonce);

            Assert.NotNull(proof);
            Assert.True(proof.Length > 0);

            // Verify proof
            var indexedMessages = new[]
            {
                new IndexedMessage { Message = messages[0], Index = 0u },
                new IndexedMessage { Message = messages[2], Index = 2u }
            };

            var verifyProofResult = BbsProvider.VerifyProof(publicKey, proof, indexedMessages, nonce);

            Assert.AreEqual(SignatureProofStatus.Success, verifyProofResult);
        }
    }
}
