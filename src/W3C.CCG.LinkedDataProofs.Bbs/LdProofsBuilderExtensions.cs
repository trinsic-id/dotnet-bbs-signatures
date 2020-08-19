using BbsDataSignatures;
using Multiformats.Base;
using W3C.CCG.LinkedDataProofs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LdProofsBuilderExtensions
    {
        public static ILdProofsBuilder AddBbsSuite(this ILdProofsBuilder builder)
        {
            builder.Services.AddSingleton<ILinkedDataSuite, BbsBlsSignature2020>();
            builder.Services.AddSingleton<ILinkedDataSuite, BbsBlsSignatureProof2020>();

            return builder;
        }
    }
}

namespace System
{
    public static class StringExtensions
    {
        public static byte[] AsBytesFromBase58(this string message) => Multibase.Base58.Decode(message);
    }
}