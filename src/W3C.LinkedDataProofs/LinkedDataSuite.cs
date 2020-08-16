using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using W3C.DidCore;
using W3C.SecurityVocabulary;

namespace W3C.LinkedDataProofs
{
    public abstract class LinkedDataSuite
    {
        public abstract IEnumerable<string> SupportedProofTypes { get; }

        public abstract JToken CreateProof(CreateProofOptions options);

        public abstract Task<JToken> CreateProofAsync(CreateProofOptions options);

        public abstract bool VerifyProof(VerifyProofOptions options);

        public abstract Task<bool> VerifyProofAsync(VerifyProofOptions options);

        protected IEnumerable<string> Canonize(JToken token)
        {
            var jsonLdParser = new JsonLdParser();
            var store = new TripleStore();
            jsonLdParser.Load(store, new StringReader(token.ToString(Newtonsoft.Json.Formatting.None)));

            var tempRdf = Path.GetTempFileName();
            var writer = new NQuadsWriter(NQuadsSyntax.Rdf11);
            writer.Save(store, tempRdf);
            var items = File.ReadAllLines(tempRdf);
            File.Delete(tempRdf);

            return items;
        }
    }
}
