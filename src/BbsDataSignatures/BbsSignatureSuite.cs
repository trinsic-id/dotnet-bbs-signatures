using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BbsSignatures;
using Newtonsoft.Json.Linq;
using W3C.DidCore;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsSignatureSuite : ILinkedDataSuite
    {
        public IEnumerable<string> SupportedProofTypes => new[] { BbsLinkedDataSignature.BbsBlsSignature2020 };

        public LinkedDataProof CreateProof(VerificationMethod key, JToken data, params object[] args)
        {
            var blsDataKey = key as BlsLinkedDataKey ?? throw new Exception("Parameter 'key' must be of type 'BlsLinkedDataKey'.");
            var messages = data as JArray ?? throw new Exception("Parameter 'data' must be of type 'JArray'.");

            var signature = BbsProvider.Sign(new SignRequest(blsDataKey.ToBlsKeyPair(), messages.ToObject<string[]>()));

            return new BbsLinkedDataSignature
            {
                ProofPurpose = ProofPurposeNames.AssertionMethod,
                Created = DateTimeOffset.Now,
                Signature = Convert.ToBase64String(signature)
            };
        }

        public Task<LinkedDataProof> CreateProofAsync(VerificationMethod key, JToken data, params object[] args) => Task.FromResult(CreateProof(key, data, args));

        public bool VerifyProof(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args)
        {
            var blsDataKey = key as BlsLinkedDataKey ?? throw new Exception("Parameter 'key' must be of type 'BlsLinkedDataKey'.");
            var messages = data as JArray ?? throw new Exception("Parameter 'data' must be of type 'JArray'.");

            var signature = Convert.FromBase64String(proof["signature"]?.Value<string>() ?? throw new Exception("Required property 'signature' was not found"));

            return BbsProvider.Verify(new VerifyRequest(blsDataKey.ToBlsKeyPair(), signature, messages.ToObject<string[]>()));
        }

        public Task<bool> VerifyProofAsync(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args) => Task.FromResult(VerifyProof(key, data, proof, args));
    }
}
