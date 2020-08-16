using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W3C.DidCore;

namespace W3C.LinkedDataProofs
{
    public interface ILinkedDataProofs
    {
        JToken CreateProof(CreateProofOptions options);

        Task<JToken> CreateProofAsync(CreateProofOptions options);
    }

    internal class DefaultLinkedDataProofs : ILinkedDataProofs
    {
        private readonly ISuiteFactory suiteFactory;

        public DefaultLinkedDataProofs(ISuiteFactory suiteFactory)
        {
            this.suiteFactory = suiteFactory;
        }

        public JToken CreateProof(CreateProofOptions options)
        {
            var suite = suiteFactory.GetSuite(options.LdSuiteType);

            return suite.CreateProof(options);
        }

        public Task<JToken> CreateProofAsync(CreateProofOptions options)
        {
            var suite = suiteFactory.GetSuite(options.LdSuiteType);

            return suite.CreateProofAsync(options);
        }
    }
}
