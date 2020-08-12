using System;
using Newtonsoft.Json.Linq;

namespace W3C.LinkedDataProofs
{
    public class ProofOptions
    {
        public JToken Input { get; set; }

        public string ProofPurpose { get; set; }

        public string VerificationMethod { get; set; }

        public DateTimeOffset? Created { get; set; }

        public IDocumentLoader DocumentLoader { get; set; }
    }
}