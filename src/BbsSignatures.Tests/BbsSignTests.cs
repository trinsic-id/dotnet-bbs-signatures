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

        [Fact(DisplayName = "Sign message")]
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
