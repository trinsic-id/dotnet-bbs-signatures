using System.Collections.ObjectModel;
using System.Linq;

namespace BbsSignatures
{
    public class BlsDeterministicPublicKey
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte> Key { get; internal set; }

        public BbsPublicKey GeneratePublicKey(uint messageCount)
        {
            unsafe
            {
                using var context = new UnmanagedMemoryContext();

                context.Reference(Key.ToArray(), out var dPublicKey);
                NativeMethods.bls_public_key_to_bbs_key(&dPublicKey, messageCount, out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                context.Dereference(publicKey, out var pk);

                return new BbsPublicKey
                {
                    Key = new ReadOnlyCollection<byte>(pk),
                    MessageCount = messageCount
                };
            }
        }
    }
}