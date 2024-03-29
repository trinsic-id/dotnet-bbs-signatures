﻿using System;
using Newtonsoft.Json.Linq;
using W3C.CCG.DidCore;
using W3C.CCG.SecurityVocabulary;

namespace W3C.CCG.LinkedDataProofs
{
    public class CreateProofOptions
    {
        /// <summary>
        /// instructs this call to compact the resulting proof to the same JSON-LD `@context` as the input
        /// document; this is the default behavior. Setting this flag to <c>false</c> can
        /// be used as an optimization to prevent an unnecessary compaction when the
        /// caller knows that all used proof terms have the same definition in the
        /// document's `@context` as the <see cref="Constants.SECURITY_CONTEXT_V2_URL"/> `@context`
        /// </summary>
        public bool CompactProof { get; set; } = true;

        public JToken Document { get; set; }

        public string LdSuiteType { get; set; }

        public string ProofPurpose { get; set; }

        public IVerificationMethod VerificationMethod { get; set; }

        public DateTimeOffset? Created { get; set; }

        public string Nonce { get; set; }

        public JToken ProofRequest { get; set; }
    }

    public class VerifyProofOptions
    {
        /// <summary>
        /// instructs this call to compact the resulting proof to the same JSON-LD `@context` as the input
        /// document; this is the default behavior. Setting this flag to <c>false</c> can
        /// be used as an optimization to prevent an unnecessary compaction when the
        /// caller knows that all used proof terms have the same definition in the
        /// document's `@context` as the <see cref="Constants.SECURITY_CONTEXT_V2_URL"/> `@context`
        /// </summary>
        public bool CompactProof { get; set; } = true;

        public JToken Document { get; set; }

        public JToken Proof { get; set; }

        public string Nonce { get; set; }

        public JToken ProofRequest { get; set; }

        public string LdSuiteType { get; set; }

        public string ProofPurpose { get; set; }
    }
}