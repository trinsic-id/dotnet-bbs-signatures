using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BbsSignatures;
using Newtonsoft.Json.Linq;
using W3C.DidCore;
using W3C.LinkedDataProofs;
using VDS.RDF.JsonLd;
using W3C;
using System.Linq;
using W3C.SecurityVocabulary;
using VDS.RDF.Writing;
using VDS.RDF;
using VDS.RDF.Parsing;
using System.IO;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace BbsDataSignatures
{
    public class BbsBlsSignature2020Suite : LinkedDataSuite
    {
        public BbsBlsSignature2020Suite(BlsKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        public BbsBlsSignature2020Suite(Bls12381VerificationKey2020 verificationKey)
        {
            KeyPair = verificationKey.ToBlsKeyPair();
        }

        public override IEnumerable<string> SupportedProofTypes => new[] { BbsBlsSignature2020.Name };

        public BlsKeyPair KeyPair { get; }

        public override JObject CreateProof(ProofOptions options)
        {
            if (KeyPair?.SecretKey == null) throw new Exception("KeyPair must contain secret key data to create proof");

            // Prepare proof
            var compactedProof = JsonLdProcessor.Compact(
                input: new BbsBlsSignature2020
                {
                    Context = Constants.SECURITY_CONTEXT_V1_URL,
                    TypeName = "https://w3c-ccg.github.io/ldp-bbs2020/context/v1#BbsBlsSignature2020"
                },
                context: new JObject(),
                options: new JsonLdProcessorOptions());

            var proof = new BbsBlsSignature2020(compactedProof)
            {
                Context = Constants.SECURITY_CONTEXT_V2_URL,
                VerificationMethod = options.VerificationMethod,
                ProofPurpose = options.ProofPurpose,
                Created = options.Created ?? DateTimeOffset.Now
            };

            var canonizedProof = Canonize(proof);
            proof.Remove("@context");

            var compactedDocument = JsonLdProcessor.Compact(options.Input, Constants.SECURITY_CONTEXT_V2_URL, new JsonLdProcessorOptions());

            var canonizedDocument = Canonize(compactedDocument);
             
            var signature = BbsProvider.Sign(new SignRequest(KeyPair, canonizedProof.Concat(canonizedDocument).ToArray()));

            var document = JObject.Parse(options.Input.ToString());
            document["proof"] = proof;
            document["proof"]["proofValue"] = Convert.ToBase64String(signature);

            return document;
        }

        public override Task<JObject> CreateProofAsync(ProofOptions options) => Task.FromResult(CreateProof(options));

        public override bool VerifyProof(JToken data, LinkedDataProof proof, params object[] args)
        {
            var messages = data as JArray ?? throw new Exception("Parameter 'data' must be of type 'JArray'.");

            var signature = Convert.FromBase64String(proof["signature"]?.Value<string>() ?? throw new Exception("Required property 'signature' was not found"));

            return BbsProvider.Verify(new VerifyRequest(KeyPair, signature, messages.ToObject<string[]>()));
        }

        public override Task<bool> VerifyProofAsync(JToken data, LinkedDataProof proof, params object[] args) => Task.FromResult(VerifyProof(data, proof, args));
    }
}
