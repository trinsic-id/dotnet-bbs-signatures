namespace BbsSignatures
{
    public class BbsSignRequest
    {
        public BbsSignRequest(BbsKeyPair keyPair, string[] messages)
        {
            KeyPair = keyPair;
            Messages = messages;
        }

        public BbsKeyPair KeyPair { get; set; }

        public string[] Messages { get; set; }
    }
}