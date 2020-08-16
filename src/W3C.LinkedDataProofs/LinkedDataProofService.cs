using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VDS.RDF.JsonLd;
using W3C.DidCore;
using W3C.SecurityVocabulary;

namespace W3C.LinkedDataProofs
{
    public interface ILinkedDataProofService
    {
        JToken CreateProof(CreateProofOptions options);

        Task<JToken> CreateProofAsync(CreateProofOptions options);
    }

    internal class DefaultLinkedDataProofService : ILinkedDataProofService
    {
        private readonly ISuiteFactory suiteFactory;

        public DefaultLinkedDataProofService(ISuiteFactory suiteFactory)
        {
            this.suiteFactory = suiteFactory;
        }

        public JToken CreateProof(CreateProofOptions options)
        {
            if (options.ProofPurpose is null) throw new Exception("Proof purpose is required.");
            if (options.LdSuiteType is null) throw new ArgumentNullException(nameof(options.LdSuiteType), "Suite type is required.");

            var suite = suiteFactory.GetSuite(options.LdSuiteType) ?? throw new Exception($"Suite not found for type '{options.LdSuiteType}'");

            var original = options.Input.DeepClone();

            if (options.CompactProof)
            {
                options.Input = JsonLdProcessor.Compact(
                    input: options.Input,
                    context: Constants.SECURITY_CONTEXT_V2_URL,
                    options: new JsonLdProcessorOptions { CompactToRelative = false });
            }

            original["proof"] = suite.CreateProof(options);

            return original;
        }

        public Task<JToken> CreateProofAsync(CreateProofOptions options)
        {
            var suite = suiteFactory.GetSuite(options.LdSuiteType);

            return suite.CreateProofAsync(options);
        }
    }
}
