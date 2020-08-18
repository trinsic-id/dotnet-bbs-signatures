using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using W3C.CCG.SecurityVocabulary;

namespace W3C.CCG.LinkedDataProofs
{
    public abstract class LinkedDataProof : JObject, ILinkedDataSuite
    {
        public LinkedDataProof()
        {
            Context = Constants.SECURITY_CONTEXT_V2_URL;
        }

        public LinkedDataProof(JObject obj) : base(obj)
        {
        }

        public LinkedDataProof(string json) : this(Parse(json))
        {
        }

        protected void EnhanceContext(JToken context)
        {
            if (Context is null)
            {
                Context = context;
                return;
            }

            switch (Context)
            {
                case JProperty _:
                case JObject _:
                    Context = new JArray
                    {
                        Context,
                        context
                    };
                    break;
                case JArray jarr:
                    jarr.Add(context);
                    break;
                default:
                    throw new Exception("Unknown context type");
            }
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

        public abstract JToken CreateProof(CreateProofOptions options, JsonLdProcessorOptions processorOptions);

        public abstract Task<JToken> CreateProofAsync(CreateProofOptions options, JsonLdProcessorOptions processorOptions);

        public abstract bool VerifyProof(VerifyProofOptions options, JsonLdProcessorOptions processorOptions);

        public abstract Task<bool> VerifyProofAsync(VerifyProofOptions options, JsonLdProcessorOptions processorOptions);

        public abstract (JToken document, JToken proof) DeriveProof(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions);

        public abstract Task<(JToken document, JToken proof)> DeriveProofAsync(DeriveProofOptions proofOptions, JsonLdProcessorOptions processorOptions);

        protected IEnumerable<string> Canonize(JToken token, JsonLdProcessorOptions options)
        {
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
