using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsBlindSignTests
    {
        [Fact]
        public void BlindSignSingleMessage()
        {
            // Secret key
            NativeMethods.bls_generate_key(ByteBuffer.None, out var _, out var secretKey, out var error);
            Assert.Equal(0, error.Code);
            var sk = secretKey.Dereference();

            // Bbs public key
            NativeMethods.bls_secret_key_to_bbs_key(sk, 1, out var publicKey, out error);
            Assert.Equal(0, error.Code);
            var pk = publicKey.Dereference();

            var handle = NativeMethods.bbs_blind_sign_context_init(out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_set_public_key(handle, pk, out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_sign_context_set_secret_key(handle, sk, out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_sign_context_set_commitment(handle, GetCommitment(pk, "test"), out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            Assert.Equal(0, error.Code);

            Assert.NotNull(blindedSignature.Dereference());
        }

        private byte[] GetCommitment(byte[] pk, params string[] message)
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);

            for (int i = 0; i < message.Length; i++)
            {
                NativeMethods.bbs_blind_commitment_context_add_message_string(handle, (uint)i, message[i], out error);
            }

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, pk, out error);

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);

            return outContext.Dereference();
        }
    }
}
