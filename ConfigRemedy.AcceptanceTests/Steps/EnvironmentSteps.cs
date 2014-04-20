using System.Linq;
using ConfigRemedy.Domain;
using NUnit.Framework;
using TechTalk.SpecFlow;

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
            });
        }

        [When(@"I GET an environment named ""(\w+)""")]
        public void WhenIGETAnEnvironmentNamed(string envName)
        {
            Result = Browser.Get("/environments/" + envName, JsonClient);
        }
        
        [When(@"I DELETE an environment named ""(.*)""")]
        public void WhenIDELETEAnEnvironmentNamed(string envName)
        {
            Result = Browser.Delete("/environments/" + envName, JsonClient);
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

        [Then(@"I should get an environment model with name ""(\w+)""")]
        public void ThenIShouldGetAnEnvironmentModelWithName(string envName)
        {
            var environment = Deserialize<Environment>(Result);
            Assert.That(environment, Is.Not.Null);
            Assert.That(environment.Name, Is.EqualTo(envName));
        }
        
        // Setup / boilerplate

        public EnvironmentSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }

    }
}