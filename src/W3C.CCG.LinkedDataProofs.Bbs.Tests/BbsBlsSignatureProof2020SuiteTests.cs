using System;
using BbsDataSignatures;
using BbsSignatures;
using FluentAssertions;
using LinkedDataProofs.Bbs.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace W3C.CCG.LinkedDataProofs.Bbs.Tests
{
    [Collection(ServiceFixture.CollectionDefinitionName)]
    public class BbsBlsSignatureProof2020SuiteTests
    {
        public BbsBlsSignatureProof2020SuiteTests(ServiceFixture serviceFixture)
        {
            Provider = serviceFixture.Provider;
            LdProofService = Provider.GetRequiredService<ILinkedDataProofService>();
        }

        public IServiceProvider Provider { get; }
        public ILinkedDataProofService LdProofService { get; }

        [Fact(DisplayName = "Sign document with BBS suite")]
        public void SignDocument()
        {
            var keyPair = BbsProvider.GenerateBlsKey();

            var document = Utilities.LoadJson("Data/test_signed_document.json");
            var proofRequest = Utilities.LoadJson("Data/test_reveal_document.json");

            var proof = LdProofService.CreateProof(new CreateProofOptions
            {
                Document = document,
                ProofRequest = proofRequest,
                LdSuiteType = BbsBlsSignatureProof2020.Name,
                ProofPurpose = ProofPurposeNames.AssertionMethod,
                VerificationMethod = keyPair.ToVerificationMethod("did:example:123#key", "did:example:123")
            });

            proof.Should().NotBeNull();
            proof["proof"].Should().NotBeNull();
            proof["proof"]["proofValue"].Should().NotBeNull();
        }
    }
}
