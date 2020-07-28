using Newtonsoft.Json.Linq;

namespace W3C.DidCore
{
    public class LinkedDataObject : JObject
    {
        public LinkedDataObject()
        {
        }

        public LinkedDataObject(JObject other) : base(other)
        {
        }

        public string Id
        {
            get => this["id"]?.Value<string>();
            set => this["id"] = value;
        }

        public string ObjectType
        {
            get => this["type"]?.Value<string>();
            set => this["type"] = value;
        }
    }
}
