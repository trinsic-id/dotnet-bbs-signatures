namespace BbsSignatures
{
    public class BbsBlsSignRequest
    {
        public BbsBlsSignRequest(BlsKeyPair keyPair, string[] messages)
        {
            KeyPair = keyPair;
            Messages = messages;
        }

        public BlsKeyPair KeyPair { get; set; }

        public string[] Messages { get; set; }
    }
}