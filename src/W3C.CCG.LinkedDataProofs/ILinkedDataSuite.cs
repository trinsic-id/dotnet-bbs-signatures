using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDS.RDF.JsonLd;

namespace W3C.CCG.LinkedDataProofs
{
    public interface ILinkedDataSuite
    {
        IEnumerable<string> SupportedProofTypes { get; }

        JToken CreateProof(CreateProofOptions options, JsonLdProcessorOptions processorOptions);

        Task<JToken> CreateProofAsync(CreateProofOptions options, JsonLdProcessorOptions processorOptions);

        bool VerifyProof(VerifyProofOptions options, JsonLdProcessorOptions processorOptions);

        Task<bool> VerifyProofAsync(VerifyProofOptions options, JsonLdProcessorOptions processorOptions);

        (JToken document, JToken proof) DeriveProof(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions);

        Task<(JToken document, JToken proof)> DeriveProofAsync(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions);
    }
}