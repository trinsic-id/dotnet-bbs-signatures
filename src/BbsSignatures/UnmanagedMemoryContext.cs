using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BbsSignatures
{
    public class UnmanagedMemoryContext : IDisposable
    {
        private bool disposedValue;

        private IList<GCHandle> GCHandlesCollection { get; set; } = new List<GCHandle>();
        private IList<ByteBuffer> UnmanagedByteBuffers { get; set; } = new List<ByteBuffer>();
        private IList<IntPtr> UnmanagedStrings { get; set; } = new List<IntPtr>();

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
        }

        internal void Dereference(ByteBuffer buffer, out byte[] data)
        {
            data = new byte[buffer.Length];
            Marshal.Copy(buffer.Data, data, 0, (int)buffer.Length);

            UnmanagedByteBuffers.Add(buffer);
        }

        internal void ThrowIfNeeded(ExternError error)
        {
            if (error.Code == 0) return;

            UnmanagedStrings.Add(error.Message);
            var data = Marshal.PtrToStringUTF8(error.Message);

            throw new BbsException(error.Code, data);
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

                foreach (var buffer in UnmanagedByteBuffers)
                {
                    NativeMethods.bbs_byte_buffer_free(buffer);
                }

                foreach (var strPtr in UnmanagedStrings)
                {
                    NativeMethods.bbs_string_free(strPtr);
                }

                GCHandlesCollection = null;
                UnmanagedByteBuffers = null;
                UnmanagedStrings = null;

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
