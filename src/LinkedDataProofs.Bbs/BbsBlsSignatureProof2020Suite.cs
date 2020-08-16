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

        public override JToken CreateProof(CreateProofOptions options)
        {
            throw new NotImplementedException();
        }

        public override Task<JToken> CreateProofAsync(CreateProofOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyProof(VerifyProofOptions options)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> VerifyProofAsync(VerifyProofOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
