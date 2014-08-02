using ConfigRemedy.AcceptanceTests.Annotations;
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

        [Then(@"response should contain ""(\w+)""")]
        public void response_should_contain(string key)
        {
            var body = Result.Body.AsString();
            Assert.That(body, Contains.Substring(key));
        }
    }
}