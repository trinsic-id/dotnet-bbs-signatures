using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsBlindCommitmentTests
    {
        [Fact(DisplayName = "Get blinded signature size")]
        public void GetBbsBlindSignatureSize()
        {
            var result = NativeMethods.bbs_blind_signature_size();

            Assert.Equal(expected: 112, actual: result);
        }

        [Fact(DisplayName = "Create blinded commitment")]
        public void BlindCommitmentSingleMessageUsingApi()
        {
            var myKey = BbsProvider.GenerateKey();
            var publicKey = myKey.GeneratePublicKey(1);

            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, "123", new[] { new IndexedMessage { Index = 0, Message = "message_0" } });

            Assert.NotNull(commitment);
            Assert.NotNull(commitment.BlindingFactor);
            Assert.NotNull(commitment.BlindSignContext);
            Assert.NotNull(commitment.Commitment);
        }
    }
}
