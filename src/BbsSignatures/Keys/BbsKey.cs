using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a BBS public key for a fixed message count
    /// </summary>
    public class BbsKey : ReadOnlyCollection<byte>
    {
        public BbsKey(IList<byte> list, uint messageCount) : base(list)
        {
            MessageCount = messageCount;
        }

        public uint MessageCount { get; internal set; }
    }
}