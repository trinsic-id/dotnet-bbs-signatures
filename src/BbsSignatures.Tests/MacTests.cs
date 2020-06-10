using System;
using System.Runtime.InteropServices;
using BbsSignatures.Bls;
using Xunit;

namespace BbsSignatures.Tests
{
    public class MacTests
    {
        [Fact]
        public void TestNoParameters()
        {
            var size = NativeMethods.bbs_blind_signature_size();
            Assert.True(size > 0);
        }

        [Fact]
        public void TestReturnExternError()
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            Assert.True(handle > 0);
            Assert.Equal(0, error.Code);
        }

        [Fact]
        public void TestInputString()
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            Assert.True(handle > 0);
            Assert.Equal(0, error.Code);

            var result = NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "1", out error);
            Assert.Equal(0, result);
            Assert.Equal(0, error.Code);
        }

        [Fact]
        public void TestInputByteBuffer()
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            Assert.True(handle > 0);
            Assert.Equal(0, error.Code);

            
            var data = new[] { (byte)1 };
            var pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            var buffer = new ByteBuffer { Length = 1, Data = pointer };

            var result = NativeMethods.bbs_blind_commitment_context_set_nonce_bytes(handle, buffer, out error);
            Assert.Equal(0, result);
            Assert.Equal(0, error.Code);

            pinnedArray.Free();
        }
    }
}
