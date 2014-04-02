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
    [Scope(Feature = "Environment")]
    public class EnvironmentSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am testing the Environment module")]
        public void GivenIAmTestingTheEnvironmentModule()
        {
            _browser = new Browser(with =>
            {
                with.Module<EnviromentModule>();
                with.Dependency(_dbContext.EmbeddedStore);
            });
        }


        [Given(@"the database is empty")]
        public void GivenTheDatabaseIsEmpty()
        {
            // TODO: Ensure?
        }

        [Given(@"an environment named ""(.*)"" exist")]
        public void GivenAnEnvironmentNamedExist(string environmentName)
        {
            WhenIPOSTAEnvironmentNamed(environmentName);
        }


        [When(@"I get available environments")]
        public void WhenIGetAvailableEnvironments()
        {
           _result = _browser.Get("/environments", JsonClient);
        }

        [When(@"I POST a environment named ""(\w+)""")]
        public void WhenIPOSTAEnvironmentNamed(string environmentName)
        {
            _result = _browser.Post("/environments", with =>
            {
                JsonClient(with);
                with.FormValue("name", environmentName);
                //var jsonBody = string.Format("{{ name: '{0}' }}", environmentName);
                //with.Body(jsonBody, "application/json");
            });
        }
        
        [When(@"I DELETE an environment named ""(.*)""")]
        public void WhenIDELETEAnEnvironmentNamed(string p0)
        {
            _result = _browser.Delete("/environments/dev", JsonClient);
        }



        [Then(@"there should be (\d+) environments")]
        public void ThenThereShouldBeEnvironments(int numberOfEnvironments)
        {
            using (var session = _dbContext.EmbeddedStore.OpenSession())
            {
                var result = session.Query<Environment>().ToList();

                Assert.That(result.Count, Is.EqualTo(numberOfEnvironments));
            }
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

        [Then(@"an environment named ""(\w+)"" should be persisted")]
        public void ThenAnEnvironmentNamedShouldBePersisted(string environmentName)
        {
            using (var session = _dbContext.EmbeddedStore.OpenSession())
            {
                var result = session
                    .Query<Environment>()
                    .Where(env => env.Name == environmentName)
                    .ToList();

                Assert.That(result, Is.Not.Empty, "Did not find the specified environment");
            }
        }

        [Then(@"location header should contain url for ""(.+)""")]
        public void ThenLocationHeaderShouldContainUrlFor(string urlPart)
        {
            Assert.That(_result.Headers.ContainsKey("Location"), Is.True, "Location header is not set");
            Assert.That(_result.Headers["Location"], Is.StringContaining(urlPart));
        }



        // Setup / boilerplate

        private readonly DatabaseContext _dbContext;
        private Browser _browser;
        private BrowserResponse _result;

        public EnvironmentSteps(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void JsonClient(BrowserContext with)
        {
            with.Header("accept", "application/json");
            //with.Header("content-type", "application/json");
            with.HttpRequest();
        }
    }
}