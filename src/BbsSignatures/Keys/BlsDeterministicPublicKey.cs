using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a deterministic BLS public key
    /// </summary>
    public class BlsDeterministicPublicKey : ReadOnlyCollection<byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlsDeterministicPublicKey"/> class.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        public BlsDeterministicPublicKey(IList<byte> list) : base(list)
        {
        }

        /// <summary>
        /// Generates a new public key with the specified <paramref name="messageCount"/>
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public BbsPublicKey CreateBbsPublicKey(uint messageCount)
        {
            unsafe
            {
                using var context = new UnmanagedMemoryContext();

                context.Reference(this.ToArray(), out var dPublicKey);
                NativeMethods.bls_public_key_to_bbs_key(dPublicKey, messageCount, out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                context.Dereference(publicKey, out var pk);

                return new BbsPublicKey(pk);
            }
        }
    }
}