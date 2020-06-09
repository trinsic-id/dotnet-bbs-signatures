using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
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
            var issuerKey = BlsSecretKey.Generate();
            var holderKey = BlsSecretKey.Generate();
            var verifierKey = BlsSecretKey.Generate();

            var issuanceNonce = "123";
            var messages = new[]
            {
                "message_1",
                "message_2",
                "message_3"
            };

            // Holder creates blind commitment and proof
            var commitment = await BbsProvider.BlindCommitmentAsync(holderKey, issuanceNonce, messages);
            Assert.NotNull(commitment);

            var proof = await BbsProvider.CreateProofAsync(holderKey, issuanceNonce, messages);
            Assert.NotNull(proof);

            // Issuer verifies proof of commited values
            var verifyBlindCommitmentResult = await BbsProvider.VerifyBlindedCommitmentAsync(proof, Array.Empty<uint>(), holderKey.GeneratePublicKey(3), issuanceNonce);
            Assert.Equal(SignatureProofStatus.Success, verifyBlindCommitmentResult);
        }
    }
}
