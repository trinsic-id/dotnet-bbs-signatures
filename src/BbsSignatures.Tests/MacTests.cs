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

        //[Fact]
        //unsafe public void TestInputByteBuffer()
        //{
        //    var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
        //    Assert.True(handle > 0);
        //    Assert.Equal(0, error.Code);

        //    using var context = new UnmanagedMemoryContext();

        //    var data = new byte[] { 1, 2, 3 };
        //    var b = context.Reference(data);
        //    var result = NativeMethods.bls_generate_key1(&b, out var pk, out var sk, out error);

        //    var ppk = context.Dereference(pk);
        //    var ssk = context.Dereference(sk);

        //    //var result = NativeMethods.bbs_blind_commitment_context_set_nonce_bytes1(handle, context.Allocate(data), out error);

        //    var ex = error.Dereference();

        //    Assert.Equal(0, result);
        //    Assert.Equal(0, error.Code);
        //}

        [StructLayout(LayoutKind.Sequential)]
        internal struct BBuffer
        {
            public ulong Length;
            public byte[] Data;
        }
    }
}
