using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W3C.DidCore;

namespace W3C.LinkedDataProofs
{
    public interface ILinkedDataSuite
    {
        IEnumerable<string> SupportedProofTypes { get; }

        LinkedDataProof CreateProof(VerificationMethod key, JToken data, params object[] args);

        Task<LinkedDataProof> CreateProofAsync(VerificationMethod key, JToken data, params object[] args);

        bool VerifyProof(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args);

        Task<bool> VerifyProofAsync(VerificationMethod key, JToken data, LinkedDataProof proof, params object[] args);
    }
}
