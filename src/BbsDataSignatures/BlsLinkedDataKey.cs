using System;
using BbsSignatures;
using Newtonsoft.Json.Linq;
using W3C.DidCore;

namespace BbsDataSignatures
{
    public class BlsLinkedDataKey : VerificationMethod
    {
        public const string TypeName = "Bls12381VerificationKey2020";

        public BlsLinkedDataKey()
        {
            VerificationMethodType = TypeName;
        }

        public string PublicKeyBase58
        {
            get => this["publicKeyBase58"]?.Value<string>();
            set => this["publicKeyBase58"] = value;
        }

        public string PrivateKeyBase58
        {
            get => this["privateKeyBase58"]?.Value<string>();
            set => this["privateKeyBase58"] = value;
        }

        internal BlsKeyPair ToBlsKeyPair()
        {
            throw new NotImplementedException();
        }
    }
}
