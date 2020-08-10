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

namespace BbsDataSignatures
{
    public class BbsBlsSignature2020Suite : ILinkedDataSuite
    {
        public BbsBlsSignature2020Suite(BlsKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        public BbsBlsSignature2020Suite(Bls12381VerificationKey2020 verificationKey)
        {
            KeyPair = verificationKey.ToBlsKeyPair();
        }

        public IEnumerable<string> SupportedProofTypes => new[] { BbsBlsSignature2020.Name };

        public BlsKeyPair KeyPair { get; }

        public LinkedDataProof CreateProof(JToken data, params object[] args)
        {
            var options = new JsonLdProcessorOptions();
            if (args[0] is Func<Uri, JsonLdLoaderOptions, RemoteDocument> loader)
            {
                options.DocumentLoader = loader;
            }

            var compacted = JsonLdProcessor.Compact(data as JObject, new JObject(), options);

            var jsonLdWriter = new JsonLdWriter(new JsonLdWriterOptions());
            var store = new TripleStore();
            var nqParser = new NQuadsParser(NQuadsSyntax.Rdf11);
            nqParser.Load(store, new StringReader(compacted.ToString()));

            if (compacted is JObject @object)
            {
                var signature = BbsProvider.Sign(new SignRequest(KeyPair, @object.Values<string>().ToArray()));

                return new BbsBlsSignature2020
                {
                    ProofPurpose = ProofPurposeNames.AssertionMethod,
                    Created = DateTimeOffset.Now,
                    Signature = Convert.ToBase64String(signature)
                };
            }
            throw new Exception("Can't create proof. Compacted result type was invalid");
        }

        public Task<LinkedDataProof> CreateProofAsync(JToken data, params object[] args) => Task.FromResult(CreateProof(data, args));

        public bool VerifyProof(JToken data, LinkedDataProof proof, params object[] args)
        {
            var messages = data as JArray ?? throw new Exception("Parameter 'data' must be of type 'JArray'.");

            var signature = Convert.FromBase64String(proof["signature"]?.Value<string>() ?? throw new Exception("Required property 'signature' was not found"));

            return BbsProvider.Verify(new VerifyRequest(KeyPair, signature, messages.ToObject<string[]>()));
        }

        public Task<bool> VerifyProofAsync(JToken data, LinkedDataProof proof, params object[] args) => Task.FromResult(VerifyProof(data, proof, args));
    }
}
