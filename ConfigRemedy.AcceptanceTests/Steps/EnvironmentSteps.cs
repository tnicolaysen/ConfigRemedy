using System.IO;
using System.Linq;
using Nancy.Testing;
using NUnit.Framework;
using Raven.Imports.Newtonsoft.Json;
using TechTalk.SpecFlow;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class EnvironmentSteps : ModuleStepsBase
    {
        [When(@"I get available environments")]
        public void WhenIGetAvailableEnvironments()
        {
            Result = Browser.Get("/environments", JsonClient);
        }

        [When(@"I POST a environment named ""(\w+)""")]
        public void WhenIPOSTAEnvironmentNamed(string environmentName)
        {
            Result = Browser.Post("/environments", with =>
            {
                JsonClient(with);
                with.FormValue("name", environmentName);
                //var jsonBody = string.Format("{{ name: '{0}' }}", environmentName);
                //with.Body(jsonBody, "application/json");
            });
        }

        [When(@"I GET an environment named ""(\w+)""")]
        public void WhenIGETAnEnvironmentNamed(string envName)
        {
            Result = Browser.Get("/environments/" + envName, JsonClient);
        }
        
        [When(@"I DELETE an environment named ""(.*)""")]
        public void WhenIDELETEAnEnvironmentNamed(string p0)
        {
            Result = Browser.Delete("/environments/dev", JsonClient);
        }

        [Then(@"there should be (\d+) environments")]
        public void ThenThereShouldBeEnvironments(int numberOfEnvironments)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session.Query<Environment>().ToList();

                Assert.That(result.Count, Is.EqualTo(numberOfEnvironments));
            }
        }

        [Then(@"an environment named ""(\w+)"" should be persisted")]
        public void ThenAnEnvironmentNamedShouldBePersisted(string environmentName)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session
                    .Query<Environment>()
                    .Where(env => env.Name == environmentName)
                    .ToList();

                Assert.That(result, Is.Not.Empty, "Did not find the specified environment");
            }
        }

        [Then(@"I should get environment in the response")]
        public void ThenIShouldGetEnvironmentInTheResponse()
        {
            // TODO: Unhack this later
            var environment = Deserialize<Environment>(Result);
            Assert.That(environment, Is.Not.Null);
        }

        private T Deserialize<T>(BrowserResponse response)
        {
            var jsonString = Result.Body.AsString();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        
        // Setup / boilerplate

        public EnvironmentSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }

    }
}