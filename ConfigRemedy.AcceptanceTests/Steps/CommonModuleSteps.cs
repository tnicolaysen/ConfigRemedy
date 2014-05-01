using ConfigRemedy.Api.Modules;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;

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