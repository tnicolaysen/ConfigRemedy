using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class SettingSteps : ModuleStepsBase
    {
        [When(@"I get available settings for the application ""(\w+)"" in the ""(\w+)"" enviroment")]
        public void WhenIGetAvailableSettingsForTheApplicationInTheEnviroment(string appName, string envName)
        {
            ///environments/{envName}/applications/{appName}/settings
            var url = string.Format("/environments/{0}/applications/{1}/settings", envName, appName);
            Result = Browser.Get(url, JsonClient);
        }


        // Setup / boilerplate

        public SettingSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}