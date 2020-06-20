using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a BBS public key for a fixed message count
    /// </summary>
    public class BbsKey : ReadOnlyCollection<byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BbsKey"/> class.
        /// </summary>
        /// <param name="list">The list to wrap.</param>
        public BbsKey(IList<byte> list) : base(list)
        {
        }

        public uint MessageCount { get; internal set; }
    }
}