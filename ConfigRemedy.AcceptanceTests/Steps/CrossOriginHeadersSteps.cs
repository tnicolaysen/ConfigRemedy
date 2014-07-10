using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Api.Infrastructure;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class CrossOriginHeadersSteps : ModuleStepsBase
    {
        [Given(@"I make a new module without any customization")]
        public void GivenIMakeANewModuleWithoutAnyCustomization()
        {
            Browser = new Browser(with =>
            {
                with.Module<TestingModule>();
                with.RequestStartup((container, pipelines, ctx) => CustomPipelines.Configure(pipelines));
            });
        }

        [Then(@"the header should contain ""(.*)"" = ""(.*)""")]
        public void ThenTheHeaderShouldContain(string headerName, string headerValue)
        {
            Assert.That(Result.Headers[headerName], Is.EqualTo(headerValue));
        }
    }

    [UsedImplicitly]
    public class TestingModule : NancyModule
    {
        public TestingModule()
        {
            Delete["/"] = _ => HttpStatusCode.NoContent;
        }
    }
}