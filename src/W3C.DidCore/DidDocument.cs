using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace W3C.DidCore
{
    public class DidDocument : JObject
    {
        public DidDocument(JObject obj) : base(obj)
        {

        }

        public DidDocument()
        {
            Context = "https://www.w3.org/ns/did/v1";
        }

        public JToken Context
        {
            get => this["@context"];
            set => this["@context"] = value;
        }

        public string Id
        {
            get => this["id"]?.Value<string>();
            set => this["id"] = value;
        }

        [JsonProperty("publicKey", NullValueHandling = NullValueHandling.Ignore)]
        public IList<VerificationMethod> PublicKey { get; set; }

        [JsonProperty("verificationMethod", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IVerificationMethod> VerificationMethod { get; set; }

        [JsonProperty("authentication", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IVerificationMethod> Authentication { get; set; }

        [JsonProperty("assertionMethod", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IVerificationMethod> AssertionMethod { get; set; }

        [JsonProperty("capabilityDelegation", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IVerificationMethod> CapabilityDelegation { get; set; }

        [JsonProperty("capabilityInvocation", NullValueHandling = NullValueHandling.Ignore)]
        public IList<IVerificationMethod> CapabilityInvocation { get; set; }

        public JToken Controller { get => this["controller"]?.Value<string>(); set => this["controller"] = value; }

        [JsonProperty("service", NullValueHandling = NullValueHandling.Ignore)]
        public IList<ServiceEndpoint> Service { get; set; }

        public DateTimeOffset? Created { get => this["created"]?.Value<DateTimeOffset>(); set => this["created"] = value; }

        public DateTimeOffset? Updated { get => this["updated"]?.Value<DateTimeOffset>(); set => this["updated"] = value; }
    }
}
