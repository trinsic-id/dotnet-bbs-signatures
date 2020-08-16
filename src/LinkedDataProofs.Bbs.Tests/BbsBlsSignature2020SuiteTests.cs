using System;
using System.IO;
using BbsDataSignatures;
using BbsSignatures;
using LinkedDataProofs.Bbs.Tests;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using W3C.LinkedDataProofs;
using W3C.SecurityVocabulary;
using Xunit;
using FluentAssertions;
using W3C.DidCore;

namespace LindedDataProofs.Bbs
{
    public class BbsBlsSignature2020SuiteTests
    {
        [Fact]
        public void DeriveProof()
        {
            var keyPair = BbsProvider.GenerateBlsKey();
            var suite = new BbsBlsSignature2020Suite(keyPair);

            var document = Utilities.LoadJson("Data/TestDocument.json");

            var proof = suite.CreateProof(new CreateProofOptions
            {
                Input = document,
                ProofPurpose = ProofPurposeNames.AssertionMethod,
                VerificationMethod = (VerificationMethodReference)"did:example:489398593#test"
            });

            proof.Should().NotBeNull();
            proof["proof"].Should().NotBeNull();
            proof["proof"]["proofValue"].Should().NotBeNull();
        }

        [Fact]
        public void JsonLdTests()
        {
            var documentLoader = new CustomDocumentLoader();
            documentLoader.Add("https://schema.org", Utilities.LoadJson("Data/schemaorgcontext.jsonld"));
            documentLoader.Add("https://w3id.org/security/v2", Contexts.SecurityContextV2);
            documentLoader.Add("https://w3id.org/security/v1", Contexts.SecurityContextV1);
            documentLoader.Add("https://w3c-ccg.github.io/ldp-bbs2020/context/v1", Utilities.LoadJson("Data/lds-bbsbls2020-v0.0.json"));

            var options = new JsonLdProcessorOptions();
            options.DocumentLoader = documentLoader.GetDocumentLoader();

            var document = new JObject
            {
                { "@context", "https://w3id.org/security/v2" },
                { "type", "https://w3c-ccg.github.io/ldp-bbs2020/context/v1#BbsBlsSignature2020" },
                { "proofPurpose", "assertionMethod" },
                { "created", "2020-08-10T17:20:20Z" },
                { "verificationMethod", "did:example:489398593#test" }
            };

            var compacted = JsonLdProcessor.Compact(document, new JObject(), options);

            using var store = new TripleStore();
            var parser = new JsonLdParser(options);
            parser.Load(store, new StringReader(document.ToString()));
        }

        [Fact]
        public void JsonLdTestsSchemaOrg()
        {
            var document = new JObject
            {
                { "@context", "https://schema.org" },
                { "@type", "Person" },
                { "givenName", "Tomislav" }
            };

            var compacted = JsonLdProcessor.Compact(document, new JObject(), new JsonLdProcessorOptions());
        }
    }
}
