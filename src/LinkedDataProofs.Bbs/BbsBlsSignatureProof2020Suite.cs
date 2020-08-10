using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsBlsSignatureProof2020Suite : LinkedDataSuite
    {
        public override IEnumerable<string> SupportedProofTypes => new[] { BbsBlsSignatureProof2020.Name };

        public override JObject CreateProof(ProofOptions options)
        {
            throw new NotImplementedException();
        }

        public override Task<JObject> CreateProofAsync(ProofOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyProof(JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> VerifyProofAsync(JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
