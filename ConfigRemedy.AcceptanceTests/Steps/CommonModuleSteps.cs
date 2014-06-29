using System;
using System.Net;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Api.Modules;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class CommonModuleSteps : ModuleStepsBase
    {
        // Givens

        [Given(@"I have a JSON client")]
        public void GivenIHaveAJSONClient()
        {
            Browser = new Browser(with =>
            {
                with.Module<EnviromentModule>();
                with.Module<ApplicationModule>();
                with.Module<SettingModule>();
                with.RequestStartup((container, pipelines, ctx) => CustomPipelines.Configure(pipelines));
                with.Dependency(DbContext.EmbeddedStore.OpenSession());
            });
        }

        [Given(@"an environment named ""(.*)"" exist")]
        public void GivenAnEnvironmentNamedExist(string environmentName)
        {
            When(string.Format(@"I POST a environment named ""{0}""", environmentName));
        }

        [Given(@"the database is empty")]
        public void GivenTheDatabaseIsEmpty()
        {
            // TODO: Ensure?
        }

        // Whens

        [When(@"I make a (\w+) request")]
        public void WhenIMakeARequest(string requestType)
        {
            if (string.Equals(requestType, "DELETE", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Delete("/", JsonClient);
            }
            else if (string.Equals(requestType, "OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Options("/", JsonClient);
            }
            else if (string.Equals(requestType, "GET", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Get("/", JsonClient);
            }
            else if (string.Equals(requestType, "POST", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Post("/", JsonClient);
            }
            else if (string.Equals(requestType, "PUT", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Put("/", JsonClient);
            }
            else if (string.Equals(requestType, "HEAD", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Head("/", JsonClient);
            }
            else
            {
                throw new ArgumentOutOfRangeException("requestType", "Request type is not supported");
            }
        }

        // Thens

        [Then(@"I should get HTTP (\w+)")]
        public void ThenIShouldGetHTTPCode(HttpStatusCode statusCode)
        {
            Assert.That(Result.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"I should get HTTP (\w+) with reason ""(.*)""")]
        public void ThenIShouldGetHTTPForbiddenWithReason(HttpStatusCode statusCode, string reason)
        {
            Assert.That(Result.StatusCode, Is.EqualTo(statusCode));
            Assert.That(Result.ReasonPhrase, Is.EqualTo(reason));
        }


        [Then(@"I should get an empty list")]
        public void ThenIShouldGetAnEmptyList()
        {
            Assert.That(Result.Body.AsString(), Is.EqualTo("[]"));
        }
        
        [Then(@"I should get an empty object")]
        public void ThenIShouldGetAnEmptyObject()
        {
            Assert.That(Result.Body.AsString(), Is.EqualTo("{}"));
        }

        [Then(@"I should get an empty body")]
        public void ThenIShouldGetAnEmptyBody()
        {
            Assert.That(Result.Body.AsString(), Is.Empty);
        }

        [Then(@"location header should contain url for ""(.+)""")]
        public void ThenLocationHeaderShouldContainUrlFor(string urlPart)
        {
            Assert.That(Result.Headers.ContainsKey("Location"), Is.True, "Location header is not set");
            Assert.That(Result.Headers["Location"], Is.StringContaining(urlPart));
        }

        // Setup / boilerplate

        public CommonModuleSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }

    }
}