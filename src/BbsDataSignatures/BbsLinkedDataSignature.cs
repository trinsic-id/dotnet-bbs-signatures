using Newtonsoft.Json.Linq;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsLinkedDataSignature : LinkedDataProof
    {
        public const string BbsBlsSignature2020 = "BbsBlsSignature2020";

        public BbsLinkedDataSignature()
        {
            ProofType = BbsBlsSignature2020;
        }

        public string Signature
        {
            get => this["signature"]?.Value<string>();
            set => this["signature"] = value;
        }
    }
}
