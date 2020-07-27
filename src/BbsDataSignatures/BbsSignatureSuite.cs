using System;
using System.Threading.Tasks;
using BbsSignatures;
using Newtonsoft.Json.Linq;
using W3C.DidCore;
using W3C.LinkedDataProofs;

namespace BbsDataSignatures
{
    public class BbsSignatureSuite : ILinkedDataSuite
    {
        public LinkedDataProof CreateProof(VerificationMethod key, JToken data, params object[] args)
        {
            var proofType = (args[0] ?? BbsLinkedDataSignature.BbsBlsSignature2020);

            switch (proofType)
            {
                case BbsLinkedDataSignature.BbsBlsSignature2020:
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
            }

            throw new Exception($"Unsupported proof type: {proofType}");
        }

        public Task<LinkedDataProof> CreateProofAsync(VerificationMethod key, JToken data, params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public bool VerifyProof(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> VerifyProofAsync(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}
