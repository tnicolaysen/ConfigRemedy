using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using Raven.Client.Linq;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class ApplicationSteps : ModuleStepsBase
    {
        [Given(@"""(\w+)"" has the application ""(\w+)""")]
        public void GivenEnvironmentHasTheApplication(string envName, string appName)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I get available applications for the ""(.*)"" enviroment")]
        public void WhenIGetAvailableApplicationsForTheEnviroment(string envName)
        {
            Result = Browser.Get("/environments/" + envName + "/applications", JsonClient);
        }

        [When(@"I POST a application named ""(.*)"" to the ""(.*)"" environment")]
        public void WhenIPOSTAApplicationNamedToTheEnvironment(string appName, string envName)
        {
            Result = Browser.Post("/environments/" + envName + "/applications/", with =>
            {
                JsonClient(with);
                with.FormValue("name", appName);
            });
        }

        [Then(@"I should get a list containing: ""(.*)"", ""(.*)""")]
        public void ThenIShouldGetAListContaining(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"an application named ""(\w+)"" should be persisted")]
        public void ThenAnApplicationNamedShouldBePersisted(string appName)
        {
            // TODO: Consider specifying query
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session
                    .Query<Environment>()
                    .Where(env => env.Applications.Any(a => a.Name == appName))
                    .ToList();

                Assert.That(result, Is.Not.Empty, "Did not find the specified application");
            }
        }


        // Setup / boilerplate

        public ApplicationSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}