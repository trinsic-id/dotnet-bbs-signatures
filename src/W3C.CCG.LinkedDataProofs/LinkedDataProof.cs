using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;

namespace W3C.CCG.LinkedDataProofs
{
    public abstract class LinkedDataProof : JObject, ILinkedDataSuite
    {
        public LinkedDataProof()
        {
        }

        public LinkedDataProof(JObject obj) : base(obj)
        {
        }

        public LinkedDataProof(string json) : this(Parse(json))
        {
        }

        public JToken Context
        {
            get => this["@context"];
            set => this["@context"] = value;
        }

        public string TypeName
        {
            get => this["type"]?.Value<string>();
            set => this["type"] = value;
        }

        public string ProofPurpose
        {
            get => this["proofPurpose"]?.Value<string>();
            set => this["proofPurpose"] = value;
        }

        public DateTimeOffset? Created
        {
            get => this["created"]?.Value<DateTimeOffset>();
            set => this["created"] = value;
        }

        public string VerificationMethod
        {
            get => this["verificationMethod"]?.Value<string>();
            set => this["verificationMethod"] = value;
        }

        public abstract IEnumerable<string> SupportedProofTypes { get; }

        public abstract JToken CreateProof(CreateProofOptions options);

        public abstract Task<JToken> CreateProofAsync(CreateProofOptions options);

        public abstract bool VerifyProof(VerifyProofOptions options);

        public abstract Task<bool> VerifyProofAsync(VerifyProofOptions options);

        protected IEnumerable<string> Canonize(JToken token, JsonLdProcessorOptions options = null)
        {
            options ??= new JsonLdProcessorOptions();

            var jsonLdParser = new JsonLdParser(options);
            var store = new TripleStore();
            jsonLdParser.Load(store, new StringReader(token.ToString(Newtonsoft.Json.Formatting.None)));

            var tempRdf = System.IO.Path.GetTempFileName();
            var writer = new NQuadsWriter(NQuadsSyntax.Rdf11);
            writer.Save(store, tempRdf);
            var items = File.ReadAllLines(tempRdf);
            File.Delete(tempRdf);

            return items;
        }
    }
}
