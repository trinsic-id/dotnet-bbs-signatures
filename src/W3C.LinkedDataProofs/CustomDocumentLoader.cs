using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VDS.RDF.JsonLd;

namespace W3C.LinkedDataProofs
{
    public class CustomDocumentLoader : IDocumentLoader
    {
        public Dictionary<Uri, RemoteDocument> Documents = new Dictionary<Uri, RemoteDocument>();

        public void Add(string uri, JObject document)
        {
            Documents.Add(new Uri(uri), new RemoteDocument { Document = document });
        }

        public Func<Uri, JsonLdLoaderOptions, RemoteDocument> GetDocumentLoader()
        {
            return (uri, options) =>
            {
                return Documents[uri];
            };
        }
    }

    public interface IDocumentLoader
    {
        Func<Uri, JsonLdLoaderOptions, RemoteDocument> GetDocumentLoader();
    }
}
