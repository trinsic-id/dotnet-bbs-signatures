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
            // Secret key
            NativeMethods.bls_generate_key(ByteBuffer.None, out var _, out var secretKey, out var error);
            Assert.Equal(0, error.Code);
            var sk = secretKey.Dereference();

            // Bbs public key
            NativeMethods.bls_secret_key_to_bbs_key(sk, 1, out var bbsPublicKey, out error);
            Assert.Equal(0, error.Code);
            var pk = bbsPublicKey.Dereference();

            var handle = NativeMethods.bbs_blind_commitment_context_init(out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, pk, out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);
            Assert.Equal(0, error.Code);

            Assert.NotNull(outContext.Dereference());
            Assert.NotNull(blindingFactor.Dereference());
        }
    }
}
