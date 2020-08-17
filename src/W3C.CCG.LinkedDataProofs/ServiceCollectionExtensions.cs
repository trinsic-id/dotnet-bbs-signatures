
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using W3C.CCG.LinkedDataProofs;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLinkedDataProofs(this IServiceCollection services, Action<ILdProofsBuilder> configure = null)
        {
            var builder = new DefaulLdProofsBuilder(services);

            configure?.Invoke(builder);

            services.TryAddSingleton<ISuiteFactory, DefaultSuiteFactory>();
            services.TryAddSingleton<ILinkedDataProofService, DefaultLinkedDataProofService>();
            services.TryAddSingleton<IDocumentLoader, CustomDocumentLoader>();

            return services;
        }
    }

    public interface ILdProofsBuilder
    {
        IServiceCollection Services { get; }
    }

    internal class DefaulLdProofsBuilder : ILdProofsBuilder
    {
        public DefaulLdProofsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}