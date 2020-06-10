using System.Collections.ObjectModel;

namespace BbsSignatures
{
    public class BbsPublicKey
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte> Key { get; internal set; }

        /// <summary>
        /// Gets the message count.
        /// </summary>
        /// <value>
        /// The message count.
        /// </value>
        public uint MessageCount { get; internal set; }
    }
}