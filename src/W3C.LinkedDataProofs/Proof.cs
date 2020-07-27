using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using W3C.DidCore;

namespace W3C.LinkedDataProofs
{
    public class LinkedDataProof : JObject
    {
        public LinkedDataProof()
        {
        }

        public LinkedDataProof(JObject obj) : base(obj)
        {
        }

        public LinkedDataProof(string json) : this(Parse(json))
        {
        }

        public string ProofType
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

        public VerificationMethodReference VerificationMethod
        {
            get => this["verificationMethod"]?.ToObject<VerificationMethodReference>();
            set => this["verificationMethod"] = value;
        }
    }
}
