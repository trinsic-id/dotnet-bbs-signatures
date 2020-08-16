using System;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public ServiceProvider Provider { get; }
    }
}
