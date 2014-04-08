using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class ApplicationSteps : ModuleStepsBase
    {
        [When(@"I get available applications for the ""(.*)"" enviroment%")]
        public void WhenIGetAvailableApplicationsForTheEnviroment(string envName)
        {
            Result = Browser.Get("/environments/" + envName + "/applications", JsonClient);
        }

        // Setup / boilerplate

        public ApplicationSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}