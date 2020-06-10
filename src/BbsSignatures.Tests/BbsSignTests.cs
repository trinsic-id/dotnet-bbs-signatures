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
            var blsKeyPair = BlsSecretKey.Generate();
            var bbsPublicKey = blsKeyPair.GeneratePublicKey(1);

            var handle = NativeMethods.bbs_sign_context_init(out var error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_add_message_string(handle, "test", out error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_set_public_key(handle, bbsPublicKey.Key, out error);
            error.ThrowOnError();

            NativeMethods.bbs_sign_context_set_secret_key(handle, blsKeyPair.Key, out error);
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
            var myKey = BlsSecretKey.Generate();
            var publiKey = myKey.GeneratePublicKey(1);

            var signature = await BbsProvider.SignAsync(myKey, publiKey, new[] { "message" });

            Assert.NotNull(signature);
            Assert.Equal(signature.Length, NativeMethods.bbs_signature_size());
        }
    }
}
