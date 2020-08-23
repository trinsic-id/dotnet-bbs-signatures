using System;
using System.Runtime.InteropServices;

namespace Hyperledger.Ursa.Bbs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ByteBuffer
    {
        public ulong Length;
        public IntPtr Data;
    }
}