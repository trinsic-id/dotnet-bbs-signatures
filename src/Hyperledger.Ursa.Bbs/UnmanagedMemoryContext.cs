﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hyperledger.Ursa.Bbs
{
    /// <summary>
    /// Provides unmanaged memory context will automatic allocation and deallocation of pointers
    /// and strings when working with FFI interface
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class UnmanagedMemoryContext : IDisposable
    {
        private bool disposedValue;

        private IList<GCHandle> GCHandlesCollection { get; set; } = new List<GCHandle>();
        private IList<ByteBuffer> UnmanagedByteBuffers { get; set; } = new List<ByteBuffer>();
        private IList<IntPtr> UnmanagedStrings { get; set; } = new List<IntPtr>();

        /// <summary>
        /// Creates a <see cref="ByteBuffer"/> from a <see cref="byte[]"/> by allocating unmanaged memory handle
        /// and assignin that pointer to the byte buffer. When this instance is disposed, the unmanaged memory handle 
        /// will be freed.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="byteBuffer">The byte buffer.</param>
        /// <exception cref="ArgumentNullException">buffer - Input buffer cannot be null.</exception>
        internal ByteBuffer ToBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer), "Input buffer cannot be null.");

            var pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            GCHandlesCollection.Add(pinnedArray);

            return new ByteBuffer { Length = (ulong)buffer.Length, Data = pointer };
        }

        /// <summary>
        /// Create a <see cref="ByteBuffer"/> from a <see cref="ReadOnlyCollection{byte}"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal ByteBuffer ToBuffer(ReadOnlyCollection<byte> buffer) => ToBuffer(buffer.ToArray());

        /// <summary>
        /// Create a <see cref="ByteBuffer"/> from a <see cref="BbsKeyPair"/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal ByteBuffer ToBuffer(BbsKeyPair keyPair) => ToBuffer(keyPair.PublicKey.ToArray());

        /// <summary>
        /// Create a <see cref="byte[]"/> from a <see cref="ByteBuffer"/>
        /// </summary>
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal byte[] ToByteArray(ByteBuffer buffer)
        {
            var data = new byte[buffer.Length];
            Marshal.Copy(buffer.Data, data, 0, (int)buffer.Length);

            UnmanagedByteBuffers.Add(buffer);

            return data;
        }

        internal ReadOnlyCollection<byte> ToReadOnlyCollection(ByteBuffer buffer) => new ReadOnlyCollection<byte>(ToByteArray(buffer));


        /// <summary>
        /// Throws <see cref="BbsException"/> if the error code isn't successful. Additionally,
        /// if erorr contains a string message, the FFI string will be freed when this instance is disposed
        /// by invoking <see cref="Native.bbs_string_free(IntPtr)"/>
        /// </summary>
        /// <param name="error">The error.</param>
        internal void ThrowIfNeeded(ExternError error)
        {
            if (error.Code == 0) return;

            UnmanagedStrings.Add(error.Message);
            var data = Marshal.PtrToStringUTF8(error.Message);

            throw new BbsException(error.Code, data);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var handle in GCHandlesCollection)
                    {
                        handle.Free();
                    }
                }

                foreach (var buffer in UnmanagedByteBuffers)
                {
                    Native.bbs_byte_buffer_free(buffer);
                }

                foreach (var strPtr in UnmanagedStrings)
                {
                    Native.bbs_string_free(strPtr);
                }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                GCHandlesCollection = null;
                UnmanagedByteBuffers = null;
                UnmanagedStrings = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnmanagedMemoryContext" /> class.
        /// </summary>
        ~UnmanagedMemoryContext() => Dispose(disposing: false);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
