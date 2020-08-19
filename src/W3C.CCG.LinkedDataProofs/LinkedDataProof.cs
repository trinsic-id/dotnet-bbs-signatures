using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Text;
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
                case JValue _:
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

        public static IEnumerable<string> ToRdf(JToken token, JsonLdProcessorOptions options)
        {
            var jsonLdParser = new JsonLdParser(options);
            var store = new TripleStore();
            jsonLdParser.Load(store, new StringReader(token.ToString(Newtonsoft.Json.Formatting.None)));

            //FixStringLiterals(store);

            var nqWriter = new NQuadsWriter(NQuadsSyntax.Rdf11);
            using var expectedTextWriter = new System.IO.StringWriter();
            nqWriter.Save(store, expectedTextWriter);
            return expectedTextWriter.ToString().Split(Environment.NewLine).Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private static void FixStringLiterals(ITripleStore store)
        {
            var xsdString = new Uri("http://www.w3.org/2001/XMLSchema#string");
            foreach (var t in store.Triples.ToList())
            {
                var literalNode = t.Object as ILiteralNode;
                if (literalNode != null && String.IsNullOrEmpty(literalNode.Language) && literalNode.DataType.Equals(xsdString))
                {
                    var graphToUpdate = t.Graph;
                    graphToUpdate.Retract(t);
                    graphToUpdate.Assert(
                        new Triple(t.Subject, t.Predicate,
                            graphToUpdate.CreateLiteralNode(literalNode.Value, xsdString),
                            graphToUpdate.BaseUri));
                }
            }
        }

        public static IEnumerable<string> Canonize(JToken token, JsonLdProcessorOptions options)
        {
            return ToRdf(token, options)
                .Select(x => x.StartsWith("_:b0") ? x.ReplaceFirst("_:b0", "_:c14n0") : x)
                .OrderBy(x => x)
                .Select(x => x.Replace("^^<http://www.w3.org/2001/XMLSchema#string>", ""));
        }

        public static JToken FromRdf(IEnumerable<string> statements, JsonLdProcessorOptions options)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in statements)
            {
                stringBuilder.AppendLine(item);
            }
            var reader = new StringReader(stringBuilder.ToString());

            var store = new TripleStore();
            var parser = new NQuadsParser(NQuadsSyntax.Rdf11);
            parser.Load(store, reader);

            var ldWriter = new JsonLdWriter();
            var stringWriter = new System.IO.StringWriter();
            ldWriter.Save(store, stringWriter);

            return JToken.Parse(stringWriter.ToString());
        }
    }
}
