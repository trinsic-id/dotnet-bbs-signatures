using System;
using Newtonsoft.Json.Linq;
using W3C.DidCore;

namespace W3C.LinkedDataProofs
{
    public class CreateProofOptions
    {
        public JToken Input { get; set; }

        public string LdSuiteType { get; set; }

        public string ProofPurpose { get; set; }

        public IVerificationMethod VerificationMethod { get; set; }

        public IDocumentLoader DocumentLoader { get; set; }

        public DateTimeOffset? Created { get; set; }
    }

    public class VerifyProofOptions
    {
        public JToken Input { get; set; }

        public JToken Proof { get; set; }

        public string ProofPurpose { get; set; }

        public VerificationMethod VerificationMethod { get; set; }

        public IDocumentLoader DocumentLoader { get; set; }
    }
}