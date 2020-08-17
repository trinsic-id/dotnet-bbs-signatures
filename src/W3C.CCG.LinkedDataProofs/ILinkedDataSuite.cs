using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace W3C.CCG.LinkedDataProofs
{
    public interface ILinkedDataSuite
    {
        IEnumerable<string> SupportedProofTypes { get; }

        JToken CreateProof(CreateProofOptions options);

        Task<JToken> CreateProofAsync(CreateProofOptions options);

        bool VerifyProof(VerifyProofOptions options);

        Task<bool> VerifyProofAsync(VerifyProofOptions options);
    }
}