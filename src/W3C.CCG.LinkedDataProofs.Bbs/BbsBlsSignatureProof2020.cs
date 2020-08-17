using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using W3C.CCG.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsBlsSignatureProof2020 : LinkedDataProof
    {
        public const string Name = "BbsBlsSignatureProof2020";

        public BbsBlsSignatureProof2020()
        {
            TypeName = Name;
            Context = "https://w3id.org/security/v2";
        }

        public BbsBlsSignatureProof2020(JObject obj) : base(obj)
        {
            TypeName ??= Name;
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

        public override IEnumerable<string> SupportedProofTypes => throw new System.NotImplementedException();

        public override JToken CreateProof(CreateProofOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override Task<JToken> CreateProofAsync(CreateProofOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override bool VerifyProof(VerifyProofOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> VerifyProofAsync(VerifyProofOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
