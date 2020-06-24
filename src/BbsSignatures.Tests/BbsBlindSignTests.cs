using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BbsSignatures.Tests
{
    public class BbsBlindSignTests
    {
        [Test(Description = "Blind sign a message using API")]
        public void BlindSignSingleMessageUsingApi()
        {
            var myKey = BbsProvider.GenerateBlsKey();
            var publicKey = myKey.GeyBbsKeyPair(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = BbsProvider.CreateBlindedCommitment(new CreateBlindedCommitmentRequest(publicKey, messages, nonce));

            var blindSign = BbsProvider.BlindSign(new BlindSignRequest(myKey, publicKey, commitment.Commitment.ToArray(), messages));

            Assert.NotNull(blindSign);
        }

        [Test(Description = "Unblind a signature")]
        public void UnblindSignatureUsingApi()
        {
            var myKey = BbsProvider.GenerateBlsKey();
            var publicKey = myKey.GeyBbsKeyPair(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = BbsProvider.CreateBlindedCommitment(new CreateBlindedCommitmentRequest(publicKey, messages, nonce));

            var blindedSignature = BbsProvider.BlindSign(new BlindSignRequest(myKey, publicKey, commitment.Commitment.ToArray(), messages));

            var result = BbsProvider.UnblindSignature(new UnblindSignatureRequest(blindedSignature, commitment.BlindingFactor.ToArray()));

            Assert.NotNull(result);
        }
    }
}
