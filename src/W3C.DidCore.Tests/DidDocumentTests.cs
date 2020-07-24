using System;
using Xunit;

namespace W3C.DidCore.Tests
{
    public class DidDocumentTests
    {
        [Fact(DisplayName = "Read and write Created and Updated values as datetime")]
        public void CreateUpdatedAsDateTimeOffset()
        {
            var didDoc = new DidDocument();

            Assert.Null(didDoc.Created);
            Assert.Null(didDoc.Updated);

            var now = DateTimeOffset.UtcNow;

            didDoc.Created = didDoc.Updated = now;

            Assert.Equal(now, didDoc.Created);
            Assert.Equal(now, didDoc.Updated);
        }
    }
}
