using System;
using Newtonsoft.Json.Linq;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsLinkedDataSignatureProof : LinkedDataProof
    {
        private const string BbsBlsSignatureProof2020 = "BbsBlsSignatureProof2020";

        public BbsLinkedDataSignatureProof()
        {
            ProofType = BbsBlsSignatureProof2020;
            Context = "https://w3id.org/security/v2";
        }

        public BbsLinkedDataSignatureProof(JObject obj) : base(obj)
        {
        }

        public JToken Context
        {
            get => this["@context"];
            set => this["@context"] = value;
        }

        public string Nonce
        {
            get => this["nonce"]?.Value<string>();
            set => this["nonce"] = value;
        }

        public string ProofValue
        {
            get => this["proofValue"]?.Value<string>();
            set => this["proofValue"] = value;
        }
    }
}
