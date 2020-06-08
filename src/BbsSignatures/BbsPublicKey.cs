namespace BbsSignatures
{
    public class BbsPublicKey
    {
        public byte[] PublicKey { get; internal set; }

        public uint MessageCount { get; internal set; }
    }
}