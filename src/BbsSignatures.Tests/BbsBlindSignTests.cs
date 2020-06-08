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
            var blsKey = BlsKeyPair.Generate();
            var bbsPublicKey = blsKey.GenerateBbsKey(1);

            var blsKeyHolder = BlsKeyPair.Generate();
            var bbsKeyHolder = blsKeyHolder.GenerateBbsKey(1);

            var handle = NativeMethods.bbs_blind_sign_context_init(out var error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_set_public_key(handle, bbsKeyHolder.PublicKey, out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_sign_context_set_secret_key(handle, blsKey.SecretKey, out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_sign_context_set_commitment(handle, GetCommitment(bbsPublicKey, "test"), out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            Assert.Equal(0, error.Code);

            Assert.NotNull(blindedSignature.Dereference());
        }

        private byte[] GetCommitment(BbsPublicKey bbsKey, params string[] message)
        {
            //var key = BlsKey.Create();
            //var bbsKey = key.GenerateBbsKey((uint)message.Length);

            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);

            for (int i = 0; i < message.Length; i++)
            {
                NativeMethods.bbs_blind_commitment_context_add_message_string(handle, (uint)i, message[i], out error);
            }

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, bbsKey.PublicKey, out error);
            error.ThrowOnError();

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);
            error.ThrowOnError();

            return outContext.Dereference();
        }
    }
}
