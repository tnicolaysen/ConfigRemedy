using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Domain;
using Nancy.Testing;
using NUnit.Framework;
using Raven.Client.Linq;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding, UsedImplicitly, MeansImplicitUse]
    public class ApplicationSteps : ModuleStepsBase
    {
        private const string ApplicationsBaseUrl = "api/applications/";

        [Given(@"an application named ""(\w+)"" exist")]
        public void GivenEnvironmentHasTheApplication(string appName)
        {
            WhenIPostAnApplicationNamed(appName);
        }

        [When(@"I get all applications")]
        public void WhenIGetAllApplications()
        {
            Result = Browser.Get(ApplicationsBaseUrl, JsonClient);
        }

        [When(@"I GET the application ""(.*)""")]
        public void WhenIGetTheApplication(string appName)
        {
            Result = Browser.Get(ApplicationsBaseUrl + appName, JsonClient);
        }

        [When(@"I get available applications")]
        public void WhenIGetAvailableApplications()
        {
            Result = Browser.Get(ApplicationsBaseUrl, JsonClient);
        }

        [When(@"I POST an application named ""(.*)""")]
        public void WhenIPostAnApplicationNamed(string appName)
        {
            Result = Browser.Post(ApplicationsBaseUrl, with =>
            {
                JsonClient(with);
                with.FormValue("name", appName);
            });
        }

        [When(@"I DELETE an app named ""(.*)""")]
        public void WhenIDeleteAnAppNamed(string appName)
        {
            Result = Browser.Delete(ApplicationsBaseUrl + appName, JsonClient);
        }

        [Then(@"there should be (\d+) apps")]
        public void ThenThereShouldBeApps(int numberOfApps)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session.Query<Application>().ToList();
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
                    .Query<Application>()
                    .Where(app => app.Name == appName)
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