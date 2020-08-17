using System;
using Microsoft.Extensions.DependencyInjection;
using W3C.CCG.LinkedDataProofs;
using Xunit;

namespace LinkedDataProofs.Bbs.Tests
{
    [CollectionDefinition(CollectionDefinitionName)]
    public class ServiceFixture : ICollectionFixture<ServiceFixture>
    {
        public const string CollectionDefinitionName = "Service Collection";

        public ServiceFixture()
        {
            var services = new ServiceCollection();
            services.AddLinkedDataProofs(builder => builder.AddBbsSuite());

            Provider = services.BuildServiceProvider();

            Provider.GetRequiredService<IDocumentLoader>()
                .AddCached("did:example:489398593#test", Utilities.LoadJson("Data/did_example_489398593_test.json"))
                .AddCached("https://w3c-ccg.github.io/ldp-bbs2020/context/v1", Utilities.LoadJson("Data/lds-bbsbls2020-v0.0.json"));
        }

        public ServiceProvider Provider { get; }
    }
}
