using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsIntegrationTests
    {
        [Fact(DisplayName = "Full end-to-end test")]
        public async Task FullDemoTest()
        {
            var key = BlsSecretKey.Generate();
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

            Assert.NotNull(signature);
            Assert.Equal(signature.Length, BbsProvider.SignatureSize);

            // Create blind commitment
            var commitment = await BbsProvider.BlindCommitmentAsync(publicKey, nonce, messages.ToArray(), new [] { 0u });

            Assert.NotNull(commitment);

            // Verify blinded commitment
            var verifyResult = await BbsProvider.VerifyBlindedCommitmentAsync(commitment.BlindSignContext.ToArray(), new [] { 0u }, publicKey, nonce);
            
            Assert.Equal(SignatureProofStatus.Success, verifyResult);

            // Blind sign
            var blindedSignature = await BbsProvider.BlindSignAsync(key, publicKey, commitment.Commitment.ToArray(), messages, new uint[] { 1, 2 });

            Assert.NotNull(blindedSignature);
            Assert.Equal(blindedSignature.Length, BbsProvider.BlindSignatureSize);

            // Unblind signature
            var unblindedSignature = await BbsProvider.UnblindSignatureAsync(blindedSignature, commitment.BlindingFactor.ToArray());

            Assert.NotNull(unblindedSignature);
            Assert.Equal(unblindedSignature.Length, BbsProvider.SignatureSize);

            // Verify signature
            var verifySignatureResult = await BbsProvider.VerifyAsync(publicKey, messages.ToArray(), unblindedSignature);

            Assert.True(verifySignatureResult);

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
