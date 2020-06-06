using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            NativeMethods.bls_generate_key(ByteBuffer.None, out var publicKey, out var secretKey, out var error);

            var sk = secretKey.Dereference();

            NativeMethods.bls_secret_key_to_bbs_key(sk, 1, out var bbsPublicKey, out error);

            var pk = bbsPublicKey.Dereference();

            var handle = NativeMethods.bbs_sign_context_init(out error);

            NativeMethods.bbs_sign_context_add_message_string(handle, "test", out error);

            NativeMethods.bbs_sign_context_set_public_key(handle, pk, out error);

            try
            {
                NativeMethods.bbs_sign_context_set_secret_key(handle, sk, out error);

                NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);

                var actual = signature.Dereference();

                Assert.NotNull(actual);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
