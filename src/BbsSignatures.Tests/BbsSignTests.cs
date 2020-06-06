using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsSignTests
    {
        [Fact]
        public void GetSignatureSize()
        {
            var result = NativeMethods.bbs_signature_size();

            Assert.Equal(112, result);
        }

        [Fact]
        public void SignSingleMessage()
        {
            NativeMethods.bls_generate_key(ByteArray.None, out var publicKey, out var secretKey, out var error);

            NativeMethods.bls_secret_key_to_bbs_key(secretKey, 1, out var bbsPublicKey, out error);

            var handle = NativeMethods.bbs_sign_context_init(out error);

            NativeMethods.bbs_sign_context_add_message_string(handle, "test", out error);

            NativeMethods.bbs_sign_context_set_public_key(handle, bbsPublicKey, out error);

            NativeMethods.bbs_sign_context_set_secret_key(handle, secretKey, out error);

            NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);

            var actual = signature.ToByteArray();

            Assert.NotNull(actual);
        }
    }
}
