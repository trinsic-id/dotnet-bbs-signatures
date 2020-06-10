using System.Collections.ObjectModel;

namespace BbsSignatures
{
    public class BlindCommitment
    {
        public ReadOnlyCollection<byte> BlindSignContext { get; internal set; }

        public ReadOnlyCollection<byte> BlindingFactor { get; internal set; }

        public ReadOnlyCollection<byte> Commitment { get; internal set; }
    }
}