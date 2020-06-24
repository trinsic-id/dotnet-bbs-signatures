using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BbsSignatures
{
    internal static class UnmanagedMemoryContextExtensions
    {
        /// <summary>
        /// Create a <see cref="ByteBuffer"/> from a <see cref="ReadOnlyCollection{byte}"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal static ByteBuffer ToBuffer(this UnmanagedMemoryContext context, ReadOnlyCollection<byte> buffer) => context.ToBuffer(buffer.ToArray());

        /// <summary>
        /// Create a <see cref="ByteBuffer"/> from a <see cref="BbsKeyPair"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal static ByteBuffer ToBuffer(this UnmanagedMemoryContext context, BbsKeyPair keyPair) => context.ToBuffer(keyPair.PublicKey.ToArray());
    }
}
