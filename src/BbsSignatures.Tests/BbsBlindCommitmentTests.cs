using System.Threading.Tasks;
using NUnit.Framework;

namespace BbsSignatures.Tests
{
    public class BbsBlindCommitmentTests
    {
        [Test(Description = "Get blinded signature size")]
        public void GetBbsBlindSignatureSize()
        {
            var result = Native.bbs_blind_signature_size();

            Assert.AreEqual(expected: 112, actual: result);
        }

        [Test(Description = "Create blinded commitment")]
        public void BlindCommitmentSingleMessageUsingApi()
        {
            var myKey = BbsProvider.GenerateBlsKey();
            var publicKey = myKey.GenerateBbsKey(1);

            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, "123", new[] { new IndexedMessage { Index = 0, Message = "message_0" } });

            Assert.NotNull(commitment);
            Assert.NotNull(commitment.BlindingFactor);
            Assert.NotNull(commitment.BlindSignContext);
            Assert.NotNull(commitment.Commitment);
        }
    }
}
