using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Api.Modules;
using ConfigRemedy.Security.Modules;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Security;
using Nancy.Testing;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    public class TokenizerMock : ITokenizer
    {
        public const string Token = "SmFtZXNCb25kDQphZG1pbnx1c2VyDQo2MzU0MjYxNTAwMTQ4MDQwNjANCk1vemlsbGEvNS4wIChXaW5kb3dzIE5UIDYuMzsgV09XNjQpIEFwcGxlV2ViS2l0LzUzNy4zNiAoS0hUTUwsIGxpa2UgR2Vja28pIENocm9tZS8zNi4wLjE5ODUuMTI1IFNhZmFyaS81MzcuMzY=:I6zbVjmlasIIGZEywNB5sJGE4PKWzPGX+2sj1vwdEyA=";
        public string Tokenize(IUserIdentity userIdentity, NancyContext context)
        {
            return Token;
        }

        public IUserIdentity Detokenize(string token, NancyContext context)
        {
            return new ConfiguratronUserIdentity
            {
                UserName = "JamesBond",
                UserId = "user/1",
                Role = "admin",
                Claims = new[] { "admin", "user"},
            };
        }
    }

    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class CommonModuleSteps : ModuleStepsBase
    {
        // Givens

        [Given(@"I have a JSON client")]
        public void GivenIHaveAJsonClient()
        {
            Browser = new Browser(with =>
            {
                with.Module<EnviromentModule>();
                with.Module<ApplicationModule>();
                with.Module<SettingModule>();
                with.Module<UsersModule>();
                with.Module<LoginModule>();
                with.ApplicationStartup((container, pipelines) => CustomPipelines.Configure(pipelines));
                with.RequestStartup((container, pipelines, ctx) => 
                    TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>())));
                with.Dependency<ITokenizer>(new TokenizerMock());
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
                Result = Browser.Delete("/", AuthenticatedJsonClient);
            }
            else if (string.Equals(requestType, "OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Options("/", AuthenticatedJsonClient);
            }
            else if (string.Equals(requestType, "GET", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Get("/", AuthenticatedJsonClient);
            }
            else if (string.Equals(requestType, "POST", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Post("/", AuthenticatedJsonClient);
            }
            else if (string.Equals(requestType, "PUT", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Put("/", AuthenticatedJsonClient);
            }
            else if (string.Equals(requestType, "HEAD", StringComparison.InvariantCultureIgnoreCase))
            {
                Result = Browser.Head("/", AuthenticatedJsonClient);
            }
            else
            {
                throw new ArgumentOutOfRangeException("requestType", "Request type is not supported");
            }
        }

        // Thens

        [Then(@"I should get HTTP (\w+)")]
        public void ThenIShouldGetHttpCode(HttpStatusCode statusCode)
        {
            Assert.That(Result.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"I should get HTTP (\w+) with reason ""(.*)""")]
        public void ThenIShouldGetHttpForbiddenWithReason(HttpStatusCode statusCode, string reason)
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