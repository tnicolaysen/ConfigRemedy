﻿using System.Collections.Generic;
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

        [Then(@"I should get a list containing ""(\w+)""")]
        public void ThenIShouldGetAListContaining(string appName)
        {
            var env = Deserialize<List<Application>>(Result);
            Assert.That(env.Any(a => a.Name == appName), Is.True,
                        "Did not find the application. Got the following: " + Result.Body.AsString());
        }

        private T Deserialize<T>(BrowserResponse response)
        {
            var jsonString = Result.Body.AsString();
            return JsonConvert.DeserializeObject<T>(jsonString);
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