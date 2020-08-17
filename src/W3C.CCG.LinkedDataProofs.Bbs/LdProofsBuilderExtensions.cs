using BbsDataSignatures;
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