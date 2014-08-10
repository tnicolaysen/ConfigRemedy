using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.AcceptanceTests.Misc;
using ConfigRemedy.Core;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class LoginSteps : ModuleStepsBase
    {
        private const string BaseUrl = "/api/login";

        [When(@"I POST username ""(\w+)"" and password ""(\w+)""")]
        public void when_I_post_username_and_password(string username, string password)
        {
            Result = Browser.Post(BaseUrl, with =>
            {
                UnauthenticatedJsonClient(with);
                with.FormValue("username", username);
                with.FormValue("password", password);
            });
        }

        [When(@"I GET login with Token in header")]
        public void when_I_get_login_with_token_in_header()
        {
            var token = string.Format(Constants.AuthorizationTokenTemplate, TokenizerMock.Token);
            Result = Browser.Get(BaseUrl,
                with => with.Header(Constants.AuthorizationHeaderName, token));
        }
        
        [When(@"I GET login with ApiKey ""(.*)"" in header")]
        public void when_I_get_login_with_api_key_in_header(string apiKey)
        {
            Result = Browser.Get(BaseUrl, with => with.Header(Constants.ApiKeyHeaderName, apiKey));
        }

        [When(@"I GET login with ApiKey ""(.*)"" as query string")]
        public void when_I_get_login_with_api_key_as_query_string(string apiKey)
        {
            Result = Browser.Get(BaseUrl, with => with.Query(Constants.ApiKeyQueryStringName, apiKey));
        }

        [When(@"I GET login without token or ApiKey")]
        public void when_I_get_login_without_token_or_api_key()
        {
            Result = Browser.Get(BaseUrl);
        }

        [Then(@"response should contain ""(\w+)""")]
        public void response_should_contain(string key)
        {
            var body = Result.Body.AsString();
            Assert.That(body, Contains.Substring(key));
        }
    }
}