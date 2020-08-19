using BbsSignatures;
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
    public class BbsBlsSignatureProof2020Suite : ILinkedDataSuite
    {
        public IEnumerable<string> SupportedProofTypes => new[] { BbsBlsSignatureProof2020.Name };

        public JToken CreateProof(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            var (document, proofs) = options.Document.GetProofs(processorOptions);
            var proof = new BbsBlsSignature2020(proofs.FirstOrDefault() ?? throw new Exception("Proof not found"));
            proof.Context = Constants.SECURITY_CONTEXT_V2_URL;

            var signature = Convert.FromBase64String(proof.ProofValue);
            var derivedProof = JsonLdProcessor.Compact(new BbsBlsSignatureProof2020(), Constants.SECURITY_CONTEXT_V2_URL, processorOptions);

            var documentStatements = BbsBlsSignature2020Suite.CreateVerifyDocumentData(document, processorOptions);
            var proofStatements = BbsBlsSignature2020Suite.CreateVerifyProofData(proof, processorOptions);

            var transformedInputDocumentStatements = documentStatements.Select(TransformNodeIdentifier).ToArray();

            var compactInputDocument = Helpers.FromRdf(transformedInputDocumentStatements);

            var revealDocument = JsonLdProcessor.Frame(compactInputDocument, options.ProofRequest, processorOptions);
            var revealDocumentStatements = BbsBlsSignature2020Suite.CreateVerifyDocumentData(revealDocument, processorOptions);

            var numberOfProofStatements = proofStatements.Count();

            var proofRevealIndicies = EnumerableFromInt(numberOfProofStatements).ToArray();
            var documentRevealIndicies = revealDocumentStatements.Select(x => Array.IndexOf(transformedInputDocumentStatements, x) + numberOfProofStatements).ToArray();

            if (documentRevealIndicies.Count() != revealDocumentStatements.Count())
            {
                throw new Exception("Some statements in the reveal document not found in original proof");
            }

            var revealIndicies = proofRevealIndicies.Concat(documentRevealIndicies);

            derivedProof["nonce"] = options.Nonce ?? Guid.NewGuid().ToString();

            //Combine all the input statements that
            //were originally signed to generate the proof
            var allInputStatements = proofStatements.Concat(documentStatements);

            var verificationMethod = BbsBlsSignature2020Suite.GetVerificationMethod(proofs.First(), processorOptions) as Bls12381VerificationKey2020;

            var outputProof = BbsProvider.CreateProof(new CreateProofRequest(
                verificationMethod.ToBlsKeyPair().GeyBbsKeyPair((uint)allInputStatements.Count()),
                GetProofMessages(allInputStatements.ToArray(), revealIndicies).ToArray(),
                signature,
                null,
                derivedProof["nonce"].ToString()));

            // Set the proof value on the derived proof
            derivedProof["proofValue"] = Convert.ToBase64String(outputProof);

            // Set the relevant proof elements on the derived proof from the input proof
            derivedProof["verificationMethod"] = proof["verificationMethod"];
            derivedProof["proofPurpose"] = proof["proofPurpose"];
            derivedProof["created"] = proof["created"];

            revealDocument["proof"] = derivedProof;

            return revealDocument;
        }

        private IEnumerable<ProofMessage> GetProofMessages(string[] allInputStatements, IEnumerable<int> revealIndicies)
        {
            for (var i = 0; i < allInputStatements.Count(); i++)
            {
                yield return new ProofMessage
                {
                    Message = allInputStatements[i],
                    ProofType = revealIndicies.Contains(i) ? ProofMessageType.Revealed : ProofMessageType.HiddenProofSpecificBlinding
                };
            }
        }

        private IEnumerable<int> EnumerableFromInt(int numberOfProofStatements, int startIndex = 0)
        {
            for (int i = 0; i < numberOfProofStatements; i++)
            {
                yield return i;
            }
        }

        private string TransformNodeIdentifier(string element)
        {
            var nodeIdentifier = element.Split(" ").First();
            if (nodeIdentifier.StartsWith("_:c14n"))
            {
                return element.Replace(nodeIdentifier, $"<urn:bnid:{nodeIdentifier}>");
            }
            return element;
        }

        public Task<JToken> CreateProofAsync(CreateProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }

        public bool VerifyProof(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotSupportedException();
        }

        public Task<bool> VerifyProofAsync(VerifyProofOptions options, JsonLdProcessorOptions processorOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}
