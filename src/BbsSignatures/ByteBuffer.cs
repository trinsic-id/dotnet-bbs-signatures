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
            Marshal.FreeHGlobal(Data);

            return data;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ByteBuffer"/> to <see cref="ByteArray"/>.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ByteArray(ByteBuffer buffer) => buffer.ToByteArray();
    }
}