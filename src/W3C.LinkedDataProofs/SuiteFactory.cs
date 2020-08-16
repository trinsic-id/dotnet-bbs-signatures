using System.Collections.Generic;
using System.Linq;

namespace W3C.LinkedDataProofs
{
    public interface ISuiteFactory
    {
        LinkedDataSuite GetSuite(string suiteType);
    }

    internal class DefaultSuiteFactory : ISuiteFactory
    {
        private readonly IEnumerable<LinkedDataSuite> suites;

        public DefaultSuiteFactory(IEnumerable<LinkedDataSuite> suites)
        {
            this.suites = suites;
        }

        public LinkedDataSuite GetSuite(string suiteType)
        {
            return suites.FirstOrDefault(x => x.SupportedProofTypes.Contains(suiteType));
        }
    }
}