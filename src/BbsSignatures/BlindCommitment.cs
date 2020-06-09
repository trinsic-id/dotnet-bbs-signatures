using System.Collections.ObjectModel;

namespace BbsSignatures
{
    public class BlindCommitment
    {
        public byte[] Context { get; set; }

        public byte[] BlindingFactor { get; set; }
        public ReadOnlyCollection<byte> Commitment { get; internal set; }
    }
}