using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsBlindSignTests
    {
        [Fact(DisplayName = "Blind sign a message using API")]
        public async Task BlindSignSingleMessageUsingApi()
        {
            var myKey = BbsProvider.Create();
            var publicKey = myKey.GeneratePublicKey(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = await BbsProvider.CreateBlindCommitmentAsync(publicKey, nonce, messages);

            var blindSign = await BbsProvider.BlindSignAsync(myKey, publicKey, commitment.Commitment.ToArray(), messages);

            Assert.NotNull(blindSign);
        }

        [Fact(DisplayName = "Unblind a signature")]
        public async Task UnblindSignatureUsingApi()
        {
            var myKey = BbsProvider.Create();
            var publicKey = myKey.GeneratePublicKey(2);

            var messages = new[]
            {
                new IndexedMessage { Index = 0, Message = "message_0" },
                new IndexedMessage { Index = 1, Message = "message_1" }
            };
            var nonce = "123";

            var commitment = await BbsProvider.CreateBlindCommitmentAsync(publicKey, nonce, messages);

            var blindSign = await BbsProvider.BlindSignAsync(myKey, publicKey, commitment.Commitment.ToArray(), messages);

            var result = await BbsProvider.UnblindSignatureAsync(blindSign, commitment.BlindingFactor.ToArray());

            Assert.NotNull(result);
        }
    }
}
