using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Core.Infrastructure;
using Raven.Client.Embedded;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly]
    public class DatabaseContext
    {
        public EmbeddableDocumentStore EmbeddedStore { get; private set; }

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