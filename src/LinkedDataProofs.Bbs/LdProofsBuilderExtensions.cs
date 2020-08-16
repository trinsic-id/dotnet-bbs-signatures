using BbsDataSignatures;
using W3C.LinkedDataProofs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LdProofsBuilderExtensions
    {
        public static ILdProofsBuilder AddBbsSuite(this ILdProofsBuilder builder)
        {
            builder.Services.AddSingleton<LinkedDataSuite, BbsBlsSignature2020Suite>();
            builder.Services.AddSingleton<LinkedDataSuite, BbsBlsSignatureProof2020Suite>();

            return builder;
        }
    }
}