using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace W3C.DidCore
{
    [JsonConverter(typeof(VerificationMethodConverter))]
    public interface IVerificationMethod
    {
    }

    public class VerificationMethod : JObject, IVerificationMethod
    {
        public VerificationMethod()
        {
        }

        public VerificationMethod(JObject obj) : base(obj)
        {

        }

        public string Id
        {
            get => this["id"]?.Value<string>();
            set => this["id"] = value;
        }

        public string VerificationMethodType
        {
            get => this["type"]?.Value<string>();
            set => this["type"] = value;
        }

        public string Controller
        {
            get => this["controller"]?.Value<string>();
            set => this["controller"] = value;
        }
    }

    public class VerificationMethodReference : JValue, IVerificationMethod
    {
        public VerificationMethodReference(string value) : base(value)
        {
        }

        public VerificationMethodReference(VerificationMethod verificationMethod) : this(verificationMethod.Id)
        {
        }
    }
}
