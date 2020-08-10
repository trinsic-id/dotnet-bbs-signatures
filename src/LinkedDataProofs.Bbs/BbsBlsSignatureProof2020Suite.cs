using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsBlsSignatureProof2020Suite : ILinkedDataSuite
    {
        public IEnumerable<string> SupportedProofTypes => new[] { BbsBlsSignatureProof2020.Name };

        public LinkedDataProof CreateProof(JToken data, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<LinkedDataProof> CreateProofAsync(JToken data, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool VerifyProof(JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyProofAsync(JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
