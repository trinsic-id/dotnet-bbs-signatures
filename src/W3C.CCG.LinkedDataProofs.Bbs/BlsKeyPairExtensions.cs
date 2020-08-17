using System;
using System.Linq;
using BbsDataSignatures;
using BbsSignatures;

namespace BbsDataSignatures
{
    public static class BlsKeyPairExtensions
    {
        public static Bls12381VerificationKey2020 ToVerificationMethod(this BlsKeyPair keyPair, string id = null, string controller = null)
        {
            var method = new Bls12381VerificationKey2020
            {
                PublicKeyBase58 = Convert.ToBase64String(keyPair.PublicKey.ToArray()),
                PrivateKeyBase58 = keyPair.SecretKey is null ? null : Convert.ToBase64String(keyPair.SecretKey.ToArray())
            };

            if (id != null) method.Id = id;
            if (controller != null) method.Controller = controller;

            return method;
        }
    }
}