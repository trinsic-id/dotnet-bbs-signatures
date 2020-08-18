using System.IO;
using BbsDataSignatures;
using BbsSignatures;
using LinkedDataProofs.Bbs.Tests;
using Newtonsoft.Json.Linq;
using VDS.RDF;
using VDS.RDF.JsonLd;
using VDS.RDF.Parsing;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using W3C.CCG.LinkedDataProofs;
using W3C.CCG.SecurityVocabulary;
using System;
using System.Diagnostics;

namespace LindedDataProofs.Bbs
{
    [Collection(ServiceFixture.CollectionDefinitionName)]
    public class BbsBlsSignature2020SuiteTests
    {
        public BbsBlsSignature2020SuiteTests(ServiceFixture serviceFixture)
        {
            Provider = serviceFixture.Provider;
            LdProofService = Provider.GetRequiredService<ILinkedDataProofService>();
        }

        public ServiceProvider Provider { get; }
        public ILinkedDataProofService LdProofService { get; }

        [Fact(DisplayName = "Sign document with BBS suite")]
        public void SignDocument()
        {
            var keyPair = BbsProvider.GenerateBlsKey();

            var document = Utilities.LoadJson("Data/test_document.json");

            var proof = LdProofService.CreateProof(new CreateProofOptions
            {
                Document = document,
                LdSuiteType = BbsBlsSignature2020.Name,
                ProofPurpose = ProofPurposeNames.AssertionMethod,
                VerificationMethod = keyPair.ToVerificationMethod("did:example:12345#test")
            });

            proof.Should().NotBeNull();
            proof["proof"].Should().NotBeNull();
            proof["proof"]["proofValue"].Should().NotBeNull();
        }

        [Fact(DisplayName = "Sign verifiable credential with BBS suite")]
        public void SignVerifiableCredential()
        {
            var keyPair = BbsProvider.GenerateBlsKey();

            var document = Utilities.LoadJson("Data/test_vc.json");

            var proof = LdProofService.CreateProof(new CreateProofOptions
            {
                Document = document,
                LdSuiteType = BbsBlsSignature2020.Name,
                ProofPurpose = ProofPurposeNames.AssertionMethod,
                VerificationMethod = keyPair.ToVerificationMethod("did:example:12345#test")
            });

            proof.Should().NotBeNull();
            proof["proof"].Should().NotBeNull();
            proof["proof"]["proofValue"].Should().NotBeNull();
        }

        [Fact(DisplayName = "Verify signed document with BBS suite")]
        public void VerifySignedDocument()
        {
            var document = Utilities.LoadJson("Data/test_signed_document.json");

            var proof = LdProofService.VerifyProof(new VerifyProofOptions
            {
                Document = document,
                LdSuiteType = BbsBlsSignature2020.Name,
                ProofPurpose = ProofPurposeNames.AssertionMethod
            });

            proof.Should().BeTrue();
        }

        [Fact]
        public void FramingTest()
        {
            var document = JObject.Parse(@"{
              '@context': 'https://w3id.org/security/v2',
              'id': 'did:example:489398593#test',
              'type': 'Ed25519Signature2018',
              'controller': 'did:example:489398593',
              'publicKeyBase58': 'oqpWYKaZD9M1Kbe94BVXpr8WTdFBNZyKv48cziTiQUeuhm7sBhCABMyYG4kcMrseC68YTFFgyhiNeBKjzdKk9MiRWuLv5H4FFujQsQK2KTAtzU8qTBiZqBHMmnLF4PL7Ytu'
            }");

            var result = JsonLdProcessor.Frame(
                document,
                new JObject
                {
                    { "@context", "https://w3id.org/security/v2" },
                    { "@embed", "@always" },
                    { "id", "did:example:489398593#test" }
                },
                new JsonLdProcessorOptions
                {
                    CompactToRelative = false,
                    ExpandContext = "https://w3id.org/security/v2"
                });

            Debug.WriteLine(result.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        [Fact]
        public void JsonLdTests()
        {
            //var documentLoader = new CustomDocumentLoader();
            //documentLoader.Add("https://schema.org", Utilities.LoadJson("Data/schemaorgcontext.jsonld"));
            //documentLoader.Add("https://w3id.org/security/v2", Contexts.SecurityContextV2);
            //documentLoader.Add("https://w3id.org/security/v1", Contexts.SecurityContextV1);
            //documentLoader.Add("https://w3c-ccg.github.io/ldp-bbs2020/context/v1", Utilities.LoadJson("Data/lds-bbsbls2020-v0.0.json"));

            //var options = new JsonLdProcessorOptions();
            //options.DocumentLoader = documentLoader.GetDocumentLoader();

            //var document = new JObject
            //{
            //    { "@context", "https://w3id.org/security/v2" },
            //    { "type", "https://w3c-ccg.github.io/ldp-bbs2020/context/v1#BbsBlsSignature2020" },
            //    { "proofPurpose", "assertionMethod" },
            //    { "created", "2020-08-10T17:20:20Z" },
            //    { "verificationMethod", "did:example:489398593#test" }
            //};

            //var compacted = JsonLdProcessor.Compact(document, new JObject(), options);

            //using var store = new TripleStore();
            //var parser = new JsonLdParser(options);
            //parser.Load(store, new StringReader(document.ToString()));
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
