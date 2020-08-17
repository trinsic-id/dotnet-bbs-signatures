using BbsSignatures;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.JsonLd;
using W3C.CCG.DidCore;
using W3C.CCG.LinkedDataProofs;
using W3C.CCG.SecurityVocabulary;

namespace BbsDataSignatures
{
    public class BbsBlsSignature2020 : LinkedDataProof
    {
        public const string Name = "BbsBlsSignature2020";

        public BbsBlsSignature2020()
        {
            TypeName = Name;
            Context = "https://w3c-ccg.github.io/ldp-bbs2020/context/v1";
        }

        public BbsBlsSignature2020(JObject obj) : base(obj)
        {
        }

        public string ProofValue
        {
            get => this["proofValue"]?.Value<string>();
            set => this["proofValue"] = value;
        }

        public override IEnumerable<string> SupportedProofTypes => new[] { Name };

        public override JToken CreateProof(CreateProofOptions options)
        {
            if (!(options.VerificationMethod is Bls12381VerificationKey2020 verificationMethod))
            {
                throw new Exception(
                    $"Invalid verification method. " +
                    $"Expected '{nameof(Bls12381VerificationKey2020)}'. " +
                    $"Found '{options.VerificationMethod?.GetType().Name}'.");
            }

            // Prepare proof
            var compactedProof = JsonLdProcessor.Compact(
                input: new BbsBlsSignature2020
                {
                    Context = Constants.SECURITY_CONTEXT_V2_URL,
                    TypeName = "https://w3c-ccg.github.io/ldp-bbs2020/context/v1#BbsBlsSignature2020"
                },
                context: Constants.SECURITY_CONTEXT_V2_URL,
                options: new JsonLdProcessorOptions
                {
                    DocumentLoader = options.DocumentLoader?.GetDocumentLoader()
                });

            var proof = new BbsBlsSignature2020(compactedProof)
            {
                Context = Constants.SECURITY_CONTEXT_V2_URL,
                VerificationMethod = options.VerificationMethod switch
                {
                    VerificationMethodReference reference => (string)reference,
                    VerificationMethod method => method.Id,
                    _ => throw new Exception("Unknown VerificationMethod type")
                },
                ProofPurpose = options.ProofPurpose,
                Created = options.Created ?? DateTimeOffset.Now
            };

            var canonizedProof = Canonize(proof);
            proof.Remove("@context");

            // Prepare document
            var canonizedDocument = Canonize(options.Input);

            var signature = BbsProvider.Sign(new SignRequest(
                keyPair: verificationMethod.ToBlsKeyPair(),
                messages: canonizedProof.Concat(canonizedDocument).ToArray()));

            proof["proofValue"] = Convert.ToBase64String(signature);
            return proof;
        }

        public override Task<JToken> CreateProofAsync(CreateProofOptions options) => Task.FromResult(CreateProof(options));

        public override bool VerifyProof(VerifyProofOptions options)
        {
            var jsonLdOptions = new JsonLdProcessorOptions
            {
                DocumentLoader = options.DocumentLoader?.GetDocumentLoader()
            };

            var verifyData = CreateVerifyData(options.Proof, options.Input, jsonLdOptions);

            var verificationMethod = GetVerificationMethod(options.Proof, jsonLdOptions);

            var signature = Convert.FromBase64String(options.Proof["proofValue"]?.Value<string>() ?? throw new Exception("Required property 'proofValue' was not found"));

            return BbsProvider.Verify(new VerifyRequest(verificationMethod.ToBlsKeyPair(), signature, verifyData.ToArray()));
        }

        public override Task<bool> VerifyProofAsync(VerifyProofOptions options) => Task.FromResult(VerifyProof(options));

        private IEnumerable<string> CreateVerifyData(JToken proof, JToken document, JsonLdProcessorOptions options)
        {
            var pr = (JObject)proof.DeepClone();

            pr.Remove("proofValue");

            var proofStatements = Canonize(pr, options);
            var documentStatements = Canonize(document, options);

            return proofStatements.Concat(documentStatements);
        }

        private Bls12381VerificationKey2020 GetVerificationMethod(JToken proof, JsonLdProcessorOptions options)
        {
            if (proof["verificationMethod"] == null) throw new Exception("Verification method is required");

            var verificationMethod = proof["verificationMethod"].Type switch
            {
                JTokenType.Object => proof["verificationMethod"]["id"],
                JTokenType.String => proof["verificationMethod"],
                _ => throw new Exception("Unexpected verification method type")
            };

            var opts = options.Clone();
            opts.CompactToRelative = false;
            opts.ExpandContext = Constants.SECURITY_CONTEXT_V2_URL;

            var result = JsonLdProcessor.Frame(
                input: verificationMethod.ToString(),
                frame: new JObject
                {
                    { "@context", Constants.SECURITY_CONTEXT_V2_URL },
                    { "@embed", "@always" },
                    { "id", verificationMethod.ToString() }
                },
                options: opts);

            return new Bls12381VerificationKey2020(result);
        }
    }
}
