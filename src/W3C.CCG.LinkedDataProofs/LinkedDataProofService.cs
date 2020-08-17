using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VDS.RDF.JsonLd;
using W3C.CCG.SecurityVocabulary;

namespace W3C.CCG.LinkedDataProofs
{
    public interface ILinkedDataProofService
    {
        JToken CreateProof(CreateProofOptions options);

        Task<JToken> CreateProofAsync(CreateProofOptions options);

        bool VerifyProof(VerifyProofOptions options);
    }

    internal class DefaultLinkedDataProofService : ILinkedDataProofService
    {
        private readonly ISuiteFactory suiteFactory;
        private readonly IDocumentLoader documentLoader;

        public DefaultLinkedDataProofService(ISuiteFactory suiteFactory, IDocumentLoader documentLoader)
        {
            this.suiteFactory = suiteFactory;
            this.documentLoader = documentLoader;
        }

        public JToken CreateProof(CreateProofOptions options)
        {
            if (options.VerificationMethod is null) throw new Exception("Verification method is required.");
            if (options.ProofPurpose is null) throw new Exception("Proof purpose is required.");
            if (options.LdSuiteType is null) throw new Exception("Suite type is required.");

            var suite = suiteFactory.GetSuite(options.LdSuiteType) ?? throw new Exception($"Suite not found for type '{options.LdSuiteType}'");

            var original = options.Input.DeepClone();

            if (options.CompactProof)
            {
                options.Input = JsonLdProcessor.Compact(
                    input: options.Input,
                    context: Constants.SECURITY_CONTEXT_V2_URL,
                    options: new JsonLdProcessorOptions
                    { 
                        CompactToRelative = false,
                        DocumentLoader = documentLoader.GetDocumentLoader()
                    });
            }

            original["proof"] = suite.CreateProof(options);

            return original;
        }

        public async Task<JToken> CreateProofAsync(CreateProofOptions options)
        {
            if (options.VerificationMethod is null) throw new Exception("Verification method is required.");
            if (options.ProofPurpose is null) throw new Exception("Proof purpose is required.");
            if (options.LdSuiteType is null) throw new Exception("Suite type is required.");

            var suite = suiteFactory.GetSuite(options.LdSuiteType) ?? throw new Exception($"Suite not found for type '{options.LdSuiteType}'");

            var original = options.Input.DeepClone();

            if (options.CompactProof)
            {
                options.Input = JsonLdProcessor.Compact(
                    input: options.Input,
                    context: Constants.SECURITY_CONTEXT_V2_URL,
                    options: new JsonLdProcessorOptions
                    {
                        CompactToRelative = false,
                        DocumentLoader = documentLoader.GetDocumentLoader()
                    });
            }

            original["proof"] = await suite.CreateProofAsync(options);

            return original;
        }

        public bool VerifyProof(VerifyProofOptions options)
        {
            var original = options.Input.DeepClone();

            if (options.CompactProof)
            {
                options.Input = JsonLdProcessor.Compact(
                    input: options.Input,
                    context: Constants.SECURITY_CONTEXT_V2_URL,
                    options: new JsonLdProcessorOptions
                    {
                        CompactToRelative = false,
                        DocumentLoader = documentLoader.GetDocumentLoader()
                    });
            }

            var proof = (JObject)options.Input["proof"].DeepClone();
            proof["@context"] = Constants.SECURITY_CONTEXT_V2_URL;

            (options.Input as JObject).Remove("proof");
            options.Proof = proof;

            var suite = suiteFactory.GetSuite(options.LdSuiteType) ?? throw new Exception($"Suite not found for type '{options.LdSuiteType}'");

            options.DocumentLoader = documentLoader;
            return suite.VerifyProof(options);
        }
    }
}
