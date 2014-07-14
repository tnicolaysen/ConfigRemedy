using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Domain;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class EnvironmentSteps : ModuleStepsBase
    {
        private const string EnvironmentBaseUrl = "/api/environments/";

        [When(@"I get available environments")]
        public void WhenIGetAvailableEnvironments()
        {
            Result = Browser.Get(EnvironmentBaseUrl, JsonClient);
        }

        [When(@"I POST a environment named ""(\w+)""")]
        public void WhenIPostAEnvironmentNamed(string environmentName)
        {
            Result = Browser.Post(EnvironmentBaseUrl, with =>
            {
                JsonClient(with);
                with.FormValue("shortName", environmentName);
            });
        }

        [When(@"I GET an environment named ""(\w+)""")]
        public void WhenIGetAnEnvironmentNamed(string envName)
        {
            Result = Browser.Get(EnvironmentBaseUrl + envName, JsonClient);
        }
        
        [When(@"I DELETE an environment named ""(.*)""")]
        public void WhenIDeleteAnEnvironmentNamed(string envName)
        {
            Result = Browser.Delete(EnvironmentBaseUrl + envName, JsonClient);
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
                    .Where(env => env.ShortName == environmentName)
                    .ToList();

                Assert.That(result, Is.Not.Empty, "Did not find the specified environment");
            }
        }

        [Then(@"I should get an environment model with name ""(\w+)""")]
        public void ThenIShouldGetAnEnvironmentModelWithName(string envName)
        {
            var environment = Deserialize<Environment>(Result);
            Assert.That(environment, Is.Not.Null);
            Assert.That(environment.ShortName, Is.EqualTo(envName));
        }

        [Then(@"I should get an the following JSON response: (.*)")]
        public void ThenIShouldGetAnTheFollowingJSONResponse(string expectedJson)
        {
            var resultJson = Result.Body.AsString();
            Assert.That(resultJson, Is.EqualTo(expectedJson).IgnoreCase);
        }

        [Then(@"body should be ""(.*)""")]
        public void ThenBodyShouldBe(string expectedBody)
        {
            var actualBody = Result.Body.AsString();
            Assert.That(actualBody, Is.EqualTo(expectedBody));
        }

        
        // Setup / boilerplate

        public EnvironmentSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }

    }
}