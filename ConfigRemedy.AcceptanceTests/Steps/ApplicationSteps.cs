using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy.Testing;
using NUnit.Framework;
using Raven.Client.Linq;
using Raven.Imports.Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class ApplicationSteps : ModuleStepsBase
    {
        [Given(@"""(\w+)"" has the application ""(\w+)""")]
        public void GivenEnvironmentHasTheApplication(string envName, string appName)
        {
            WhenIPOSTAApplicationNamedToTheEnvironment(appName, envName);
            //When(string.Format(@"When I POST a application named ""{0}"" to the ""{1}"" environment", appName, envName));
        }

        [When(@"I GET the application ""(.*)"" in the ""(.*)"" environment")]
        public void WhenIGETTheApplicationInTheEnvironment(string appName, string envName)
        {
            Result = Browser.Get("/environments/" + envName + "/" + appName, JsonClient);
        }


        [When(@"I get available applications for the ""(.*)"" enviroment")]
        public void WhenIGetAvailableApplicationsForTheEnviroment(string envName)
        {
            Result = Browser.Get("/environments/" + envName + "/applications", JsonClient);
        }

        [When(@"I POST a application named ""(.*)"" to the ""(.*)"" environment")]
        public void WhenIPOSTAApplicationNamedToTheEnvironment(string appName, string envName)
        {
            Result = Browser.Post("/environments/" + envName + "/applications", with =>
            {
                JsonClient(with);
                with.FormValue("name", appName);
            });
        }

        [When(@"I DELETE an app named ""(.*)"" in the environment ""(.*)""")]
        public void WhenIDELETEAnAppNamedInTheEnvironment(string appName, string envName)
        {
            Result = Browser.Delete("/environments/" + envName + "/" + appName, JsonClient);
        }

        [Then(@"there should be (\d+) apps in the environment ""(\w+)""")]
        public void ThenThereShouldBeAppsInTheEnvironment(int numberOfApps, string envName)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session.Query<Environment>()
                                    .Single(e => e.Name == envName)
                                    .Applications;

                Assert.That(result.Count, Is.EqualTo(numberOfApps));
            }
        }


        [Then(@"I should get a list containing ""(\w+)""")]
        public void ThenIShouldGetAListContaining(string appName)
        {
            var apps = Deserialize<List<Application>>(Result);
            Assert.That(apps.Any(a => a.Name == appName), Is.True,
                        "Did not find the application. Got the following: " + Result.Body.AsString());
        }

        [Then(@"I should get an application model with name ""(\w+)""")]
        public void ThenIShouldGetAnApplicationModelWithName(string appName)
        {
            var app = Deserialize<Application>(Result);
            Assert.That(app, Is.Not.Null);
            Assert.That(app.Name, Is.EqualTo(appName));
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