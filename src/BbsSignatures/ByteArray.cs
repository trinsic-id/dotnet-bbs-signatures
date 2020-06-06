using System;
using System.Runtime.InteropServices;

namespace BbsSignatures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ByteArray
    {
        public uint Length;

        public byte[] Data;

        /// <summary>
        /// Create new <see cref="ByteArray"/> from input array.
        /// </summary>
        /// <param name="array">The input array. Can be <c>null</c>.</param>
        /// <returns></returns>
        internal static ByteArray Create(byte[] array = null) => new ByteArray { Data = array, Length = array is null ? 0 : (uint)array.Length };

        /// <summary>
        /// Empty byte array
        /// </summary>
        internal static ByteArray None => Create();

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte[]"/> to <see cref="ByteArray"/>.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ByteArray(byte[] d) => Create(d);
    }
}