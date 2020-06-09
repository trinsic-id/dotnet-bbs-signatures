using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsBlindSignTests
    {
        [Fact]
        public void BlindSignSingleMessage()
        {
            var blsKey = BlsSecretKey.Generate();
            var bbsPublicKey = blsKey.GeneratePublicKey(1);

            var blsKeyHolder = BlsSecretKey.Generate();
            var bbsKeyHolder = blsKeyHolder.GeneratePublicKey(1);

            var handle = NativeMethods.bbs_blind_sign_context_init(out var error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_add_message_string(handle, 0, "test", out error);
            Assert.Equal(0, error.Code);

            NativeMethods.bbs_blind_sign_context_set_public_key(handle, bbsKeyHolder.Key, out error);
            Assert.Equal(0, error.Code);
            NativeMethods.bbs_blind_sign_context_set_secret_key(handle, blsKey.Key, out error);
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
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, bbsKey.Key, out error);
            error.ThrowOnError();

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);
            error.ThrowOnError();

            return outContext.Dereference();
        }

        [Fact]
        public async Task BlindSignSingleMessageUsingApi()
        {
            var myKey = BlsSecretKey.Generate();
            var theirKey = myKey.GetDeterministicPublicKey();

            var blindSign = await BbsProvider.BlindSignAsync(myKey, theirKey, "123", new[] { "message_0", "message_1" });

            Assert.NotNull(blindSign);
        }
    }
}
