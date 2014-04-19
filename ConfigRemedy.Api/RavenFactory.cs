using Raven.Client;
using Raven.Client.Embedded;

namespace ConfigRemedy.Api
{
    public class RavenFactory
    {
        public static IDocumentStore Create()
        {
            var docstore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data"
            };

            RavenConfigurator.Configure(docstore);

            docstore.Initialize();
            return docstore;
        }

      
    }
}