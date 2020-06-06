using System;
using System.Runtime.InteropServices;

namespace BbsSignatures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ExternError
    {
        internal int Code;
        internal IntPtr Message;

        public BbsException ToException()
        {
            var data = Marshal.PtrToStringUTF8(Message);
            Marshal.FreeHGlobal(Message);

            return new BbsException(Code, data);
        }
    }
}