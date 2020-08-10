using Newtonsoft.Json.Linq;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsBlsSignature2020 : LinkedDataProof
    {
        public const string Name = "BbsBlsSignature2020";

        public BbsBlsSignature2020()
        {
            ProofType = Name;
        }

        public string Signature
        {
            get => this["signature"]?.Value<string>();
            set => this["signature"] = value;
        }
    }
}
