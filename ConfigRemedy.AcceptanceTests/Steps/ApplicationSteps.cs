using System;
using System.Linq;
using ConfigRemedy.Api.Modules;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Environment = ConfigRemedy.Models.Environment;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "Application")]
    public class ApplicationSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am testing the Application module")]
        public void GivenIAmTestingTheApplicationModule()
        {
        }


        [When(@"I get available applications for the ""(\w+)"" enviroment")]
        public void WhenIGetAvailableApplicationsForTheEnviroment(string envName)
        {
            _result = _browser.Get("/environments/" + envName + "/applications", JsonClient);
        }


        [Then(@"I should get HTTP (\w+)")]
        public void ThenIShouldGetHTTPCode(HttpStatusCode statusCode)
        {
            Assert.That(_result.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"I should get an empty list")]
        public void ThenIShouldGetAnEmptyList()
        {
            Assert.That(_result.Body.AsString(), Is.EqualTo("[]"));
        }

        // Setup / boilerplate

        private readonly DatabaseContext _dbContext;
        private Browser _browser;
        private BrowserResponse _result;

        public ApplicationSteps(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void JsonClient(BrowserContext with)
        {
            with.Header("accept", "application/json");
            with.HttpRequest();
        }
    }
}