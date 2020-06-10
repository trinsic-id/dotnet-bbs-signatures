using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
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

        [Fact(DisplayName = "Create blind commitment")]
        public void BlindCommitmentSingleMessage()
        {
            var keyPair = BlsSecretKey.Generate();
            var bbsPublicKey = keyPair.GeneratePublicKey(1);

            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, bbsPublicKey.Key, out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var commitment, out var outContext, out var blindingFactor, out error);
            Assert.Equal(0, error.Code);

            Assert.NotNull(commitment.Dereference());
            Assert.NotNull(outContext.Dereference());
            Assert.NotNull(blindingFactor.Dereference());
        }

        [Fact(DisplayName = "Create blind commitment using API")]
        public async Task BlindCommitmentSingleMessageUsingApi()
        {
            var myKey = BlsSecretKey.Generate();
            var publicKey = myKey.GeneratePublicKey(1);

            var commitment = await BbsProvider.CreateBlindCommitmentAsync(publicKey, "123", new[] { new IndexedMessage { Index = 0, Message = "message_0" } });

            Assert.NotNull(commitment);
            Assert.NotNull(commitment.BlindingFactor);
            Assert.NotNull(commitment.BlindSignContext);
            Assert.NotNull(commitment.Commitment);
        }
    }
}
