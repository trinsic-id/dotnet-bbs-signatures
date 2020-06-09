using BbsSignatures.Bls;
using System.Collections.ObjectModel;

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
            NativeMethods.bls_public_key_to_bbs_key(Key, messageCount, out var publicKey, out var error);
            error.ThrowOnError();

            return new BbsPublicKey
            {
                Key = new ReadOnlyCollection<byte>(publicKey.Dereference()),
                MessageCount = messageCount
            };
        }
    }
}