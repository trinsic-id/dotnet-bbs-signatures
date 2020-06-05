using System;
using System.Runtime.InteropServices;

namespace BbsSignatures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ByteBuffer
    {
        public long Length;
        public IntPtr Data;

        public byte[] ToByteArray()
        {
            var data = new byte[Length];
            Marshal.Copy(Data, data, 0, (int)Length);

            // Not sure if freeing the memory is required
            //Marshal.FreeHGlobal(Data);

            return data;
        }
    }
}