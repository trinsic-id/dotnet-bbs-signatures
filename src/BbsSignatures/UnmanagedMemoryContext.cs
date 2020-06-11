using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BbsSignatures.Bls;

namespace BbsSignatures
{
    public class UnmanagedMemoryContext : IDisposable
    {
        private bool disposedValue;

        private IList<GCHandle> GCHandlesCollection { get; set; } = new List<GCHandle>();

        private IList<ByteBuffer> UnmanagedBuffers { get; set; } = new List<ByteBuffer>();
        private IList<ByteBuffer> StrongReferences { get; set; } = new List<ByteBuffer>();

        public UnmanagedMemoryContext()
        {
        }

        internal void Reference(byte[] buffer, out ByteBuffer byteBuffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer), "Input buffer cannot be null.");

            var pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            byteBuffer = new ByteBuffer { Length = (ulong)buffer.Length, Data = pointer };

            GCHandlesCollection.Add(pinnedArray);
            StrongReferences.Add(byteBuffer);
        }

        internal void Dereference(ByteBuffer buffer, out byte[] data)
        {
            data = new byte[buffer.Length];
            Marshal.Copy(buffer.Data, data, 0, (int)buffer.Length);

            UnmanagedBuffers.Add(buffer);
        }

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

                foreach (var buffer in UnmanagedBuffers)
                {
                    NativeMethods.bbs_byte_buffer_free(buffer);
                }

                GCHandlesCollection = null;
                UnmanagedBuffers = null;
                StrongReferences = null;

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~UnmanagedMemoryContext()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
