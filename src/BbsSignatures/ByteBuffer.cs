using System;
using System.Runtime.InteropServices;

namespace BbsSignatures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ByteBuffer
    {
        public ulong Length;
        public IntPtr Data;

        /// <summary>
        /// Gets the none.
        /// </summary>
        /// <value>
        /// The none.
        /// </value>
        public static ByteBuffer None => new ByteBuffer { Data = IntPtr.Zero };

        /// <summary>
        /// Dereferences this instance.
        /// </summary>
        /// <returns></returns>
        public byte[] Dereference()
        {
            var data = new byte[Length];

            Marshal.Copy(Data, data, 0, (int)Length);
            Marshal.FreeHGlobal(Data);

            return data;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte[]"/> to <see cref="ByteBuffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ByteBuffer(byte[] buffer)
        {
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    return new ByteBuffer
                    {
                        Data = (IntPtr)ptr,
                        Length = (ulong)buffer.Length
                    };
                }
            }

            // Alternatively

            //GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            //IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            //// Do your stuff...
            //pinnedArray.Free();
        }
    }
}