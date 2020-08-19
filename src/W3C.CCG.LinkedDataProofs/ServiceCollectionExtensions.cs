
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using VDS.RDF.JsonLd;
using W3C.CCG.LinkedDataProofs;
using W3C.CCG.SecurityVocabulary;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLinkedDataProofs(this IServiceCollection services, Action<ILdProofsBuilder> configure = null)
        {
            var builder = new DefaultLdProofsBuilder(services);

            configure?.Invoke(builder);

            services.TryAddSingleton<ISuiteFactory, DefaultSuiteFactory>();
            services.TryAddSingleton<ILinkedDataProofService, DefaultLinkedDataProofService>();
            services.TryAddSingleton<IDocumentLoader, CustomDocumentLoader>();

            return services;
        }
    }

    public static class JTokenExtensions
    {
        public static (JToken, IEnumerable<JObject>) GetProofs(this JToken document, JsonLdProcessorOptions options, bool compactProof = true, string proofPropertyName = "proof")
        {
            if (compactProof)
            {
                document = JsonLdProcessor.Compact(document, Constants.SECURITY_CONTEXT_V2_URL, options);
            }
            var proofs = document[proofPropertyName];
            (document as JObject).Remove();

            return (document, proofs switch
            {
                JObject _ => new[] { proofs as JObject },
                JArray _ => proofs.Select(x => x as JObject),
                _ => throw new Exception("Unexpected proof type")
            });
        }
    }

    public interface ILdProofsBuilder
    {
        IServiceCollection Services { get; }
    }

    internal class DefaultLdProofsBuilder : ILdProofsBuilder
    {
        public DefaultLdProofsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}