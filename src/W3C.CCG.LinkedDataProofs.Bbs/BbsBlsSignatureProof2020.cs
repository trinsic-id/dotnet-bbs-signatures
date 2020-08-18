using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDS.RDF.JsonLd;
using W3C.CCG.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsBlsSignatureProof2020 : LinkedDataProof
    {
        public const string Name = "BbsBlsSignatureProof2020";

        public BbsBlsSignatureProof2020() : base()
        {
            TypeName = Name;
            EnhanceContext("https://w3c-ccg.github.io/ldp-bbs2020/context/v1");
        }

        public BbsBlsSignatureProof2020(JObject obj) : base(obj)
        {
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

        public override IEnumerable<string> SupportedProofTypes => throw new System.NotImplementedException();

        public override JToken CreateProof(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public override Task<JToken> CreateProofAsync(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public override (JToken document, JToken proof) DeriveProof(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(JToken document, JToken proof)> DeriveProofAsync(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public override bool VerifyProof(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotSupportedException();
        }

        public override Task<bool> VerifyProofAsync(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}
