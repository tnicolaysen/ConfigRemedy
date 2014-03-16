using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace ConfigRemedy.Api
{
    [TestFixture]
    public class EnviromentModuleTest
    {
        [Test]
        public void Should_()
        {
            // Given
            var browser = new Browser(with =>
            {
                with.Module<EnviromentModule>();
            });

            // When
            var result = browser.Get("/environments/", with =>
            {
                with.Header("content-type", "application/json");
                with.Header("accept", "application/json");
                with.HttpRequest();
            });

            // Then
            Assert.That(HttpStatusCode.OK, Is.EqualTo(result.StatusCode));
        }
    }
}