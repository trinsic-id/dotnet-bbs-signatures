using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsSignTests
    {
        [Fact(DisplayName = "Get signature size")]
        public void GetSignatureSize()
        {
            var result = NativeMethods.bbs_signature_size();

            Assert.Equal(112, result);
        }

        [Fact(DisplayName = "Sign single message")]
        public void SignSingleMessage()
        {
            var blsKeyPair = BlsKeyPair.Generate();
            var bbsPublicKey = blsKeyPair.GenerateBbsKey(1);

            var handle = NativeMethods.bbs_sign_context_init(out var error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_add_message_string(handle, "test", out error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_set_public_key(handle, bbsPublicKey.PublicKey, out error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_set_secret_key(handle, blsKeyPair.SecretKey, out error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);
            error.ThrowOnError();

            var actual = signature.Dereference();

            Assert.NotNull(actual);
            Assert.Equal(actual.Length, NativeMethods.bbs_signature_size());
        }

        [Fact]
        public async Task SignSingleMessageUsingApi()
        {
            var myKey = BlsKeyPair.Generate();
            var theirKey = BlsKeyPair.Generate();

            var signature = await BbsProvider.SignAsync(myKey, theirKey, new[] { "message" });

            Assert.NotNull(signature);
            Assert.Equal(signature.Length, NativeMethods.bbs_signature_size());
        }
    }
}
