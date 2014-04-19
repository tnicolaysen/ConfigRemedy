using ConfigRemedy.Api;
using Raven.Client.Embedded;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    public class DatabaseContext
    {
        public EmbeddableDocumentStore EmbeddedStore { get; set; }

        public DatabaseContext()
        {
            EmbeddedStore = CreateTestDatabase();
        }

        private static EmbeddableDocumentStore CreateTestDatabase()
        {   
            var embeddedStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };

            RavenConfigurator.Configure(embeddedStore);

            embeddedStore.Initialize();
            return embeddedStore;
        }
    }
}