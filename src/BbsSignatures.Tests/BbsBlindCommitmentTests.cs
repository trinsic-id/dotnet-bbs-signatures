using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsBlindCommitmentTests
    {
        [Fact]
        public void GetBbsBlindSignatureSize()
        {
            var result = NativeMethods.bbs_blind_signature_size();

            Assert.Equal(expected: 112, actual: result);
        }

        [Fact]
        public void BlindSignatureSingleMessage()
        {
            var keyPair = BlsKeyPair.Generate();
            var bbsPublicKey = keyPair.GenerateBbsKey(1);

            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, bbsPublicKey.PublicKey, out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);
            Assert.Equal(0, error.Code);

            Assert.NotNull(outContext.Dereference());
            Assert.NotNull(blindingFactor.Dereference());
        }
    }
}
