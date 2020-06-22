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
            var publicKey = myKey.GenerateBbsKey(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, nonce, messages);

            var blindSign = BbsProvider.BlindSign(myKey, publicKey, commitment.Commitment.ToArray(), messages);

            Assert.NotNull(blindSign);
        }

        [Test(Description = "Unblind a signature")]
        public void UnblindSignatureUsingApi()
        {
            var myKey = BbsProvider.GenerateBlsKey();
            var publicKey = myKey.GenerateBbsKey(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = BbsProvider.CreateBlindedCommitment(publicKey, nonce, messages);

            var blindSign = BbsProvider.BlindSign(myKey, publicKey, commitment.Commitment.ToArray(), messages);

            var result = BbsProvider.UnblindSignature(blindSign, commitment.BlindingFactor.ToArray());

            Assert.NotNull(result);
        }
    }
}
