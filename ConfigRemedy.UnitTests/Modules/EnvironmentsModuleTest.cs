using ConfigRemedy.Api;
using ConfigRemedy.Api.Modules;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace ConfigRemedy.UnitTests.Modules
{
    [TestFixture]
    public class EnviromentModuleTest
    {
        [Test]
        public void Should_return_HTTP_OK_when_database_is_empty()
        {
            var embeddedStore = CreateTestDatabase();

            // Given
            var browser = new Browser(with =>
            {
                with.Module<EnviromentModule>();
                with.Dependency(embeddedStore);
            });

            // When
            BrowserResponse result = browser.Get("/environments/", JsonClient);

            // Then
            Assert.That(HttpStatusCode.OK, Is.EqualTo(result.StatusCode));
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

        private void JsonClient(BrowserContext with)
        {
            with.Header("content-type", "application/json");
            with.Header("accept", "application/json");
            with.HttpRequest();
        }
    }
}