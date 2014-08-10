using ConfigRemedy.Core.Configuration.Settings;
using Raven.Client;
using Raven.Client.Embedded;

namespace ConfigRemedy.Repository.Infrastructure
{
    public static class RavenFactory
    {
        public static IDocumentStore Create()
        {
            var docstore = new EmbeddableDocumentStore
            {
                DataDirectory = Settings.DbPath,
                UseEmbeddedHttpServer = true,
            };

            RavenConfigurator.Configure(docstore);

            docstore.Initialize();
            return docstore;
        }
    }
}