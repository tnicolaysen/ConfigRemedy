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
            embeddedStore.Initialize();
            return embeddedStore;
        }
    }
}