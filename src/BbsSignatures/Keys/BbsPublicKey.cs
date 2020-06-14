using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a BBS public key for a fixed message count
    /// </summary>
    public class BbsPublicKey : ReadOnlyCollection<byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BbsPublicKey"/> class.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        public BbsPublicKey(IList<byte> list) : base(list)
        {
        }
    }
}