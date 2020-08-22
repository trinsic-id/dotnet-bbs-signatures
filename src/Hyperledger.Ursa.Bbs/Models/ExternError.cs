using System;
using System.Runtime.InteropServices;

namespace Hyperledger.Ursa.Bbs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ExternError
    {
        internal int Code;
        internal IntPtr Message;
    }
}