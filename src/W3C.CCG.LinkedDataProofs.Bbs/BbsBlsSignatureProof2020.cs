using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using W3C.CCG.LinkedDataProofs;
using W3C.CCG.SecurityVocabulary;

namespace BbsDataSignatures
{
    public class BbsBlsSignatureProof2020 : LinkedDataProof
    {
        public const string Name = "BbsBlsSignatureProof2020";

        public BbsBlsSignatureProof2020() : base()
        {
            TypeName = Name;
            EnhanceContext("https://w3c-ccg.github.io/ldp-bbs2020/context/v1");
        }

        public BbsBlsSignatureProof2020(JObject obj) : base(obj)
        {
        }

        public string Nonce
        {
            get => this["nonce"]?.Value<string>();
            set => this["nonce"] = value;
        }

        public string ProofValue
        {
            get => this["proofValue"]?.Value<string>();
            set => this["proofValue"] = value;
        }

        public override IEnumerable<string> SupportedProofTypes => throw new System.NotImplementedException();

        public override JToken CreateProof(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            var (document, proofs) = options.Document.GetProofs(processorOptions);
            var proof = new BbsBlsSignature2020(proofs.FirstOrDefault() ?? throw new Exception("Proof not found"));

            var signature = Convert.FromBase64String(proof.ProofValue);
            var derivedProof = JsonLdProcessor.Compact(new BbsBlsSignatureProof2020(), Constants.SECURITY_CONTEXT_V2_URL, processorOptions);

            var statements = BbsBlsSignature2020.CreateVerifyData(proof, document, processorOptions);

            var compactedDocument = FromRdf(statements, processorOptions);
            var revealDocument = JsonLdProcessor.Frame(compactedDocument, options.ProofRequest, processorOptions);

            return null;
        }

        private JToken FromRdf(IEnumerable<string> documentStatements, JsonLdProcessorOptions processorOptions)
        {
            var nqParser = new NQuadsParser(NQuadsSyntax.Rdf11);
            var input = new TripleStore();
            var tempRdf = System.IO.Path.GetTempFileName();
            File.WriteAllLines(tempRdf, documentStatements);
            nqParser.Load(input, tempRdf);
            File.Delete(tempRdf);

            var jsonLdWriter = new JsonLdWriter();
            return jsonLdWriter.SerializeStore(input);
        }

        public override Task<JToken> CreateProofAsync(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public override bool VerifyProof(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotSupportedException();
        }

        public override Task<bool> VerifyProofAsync(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}
