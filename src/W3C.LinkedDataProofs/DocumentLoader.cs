using System;
using VDS.RDF.JsonLd;

namespace W3C.LinkedDataProofs
{
    public class DocumentLoader : IDocumentLoader
    {
        public Func<Uri, JsonLdLoaderOptions, RemoteDocument> GetDocumentLoader()
        {
            throw new NotImplementedException();
        }
    }
}