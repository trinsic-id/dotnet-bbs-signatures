using System;
using BbsDataSignatures;
using BbsSignatures;
using LinkedDataProofs.Bbs.Tests;
using W3C.LinkedDataProofs;
using W3C.SecurityVocabulary;
using Xunit;

namespace LindedDataProofs.Bbs
{
    public class BbsBlsSignature2020SuiteTests
    {
        [Fact]
        public void DeriveProof()
        {
            var keyPair = BbsProvider.GenerateBlsKey();
            var suite = new BbsBlsSignature2020Suite(keyPair);

            var documentLoader = new CustomDocumentLoader();
            documentLoader.Add("https://schema.org", Utilities.LoadJson("Data/schemaorgcontext.jsonld"));
            documentLoader.Add("https://w3id.org/security/v2", Contexts.SecurityContextV2);
            documentLoader.Add("https://w3c-ccg.github.io/ldp-bbs2020/context/v1", Utilities.LoadJson("Data/lds-bbsbls2020-v0.0.json"));

            var document = Utilities.LoadJson("Data/TestDocument.json");

            var proof = suite.CreateProof(document, documentLoader.GetDocumentLoader());
        }
    }
}
