using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class SettingSteps : ModuleStepsBase
    {
        [Given(@"I POST the following setting to ""(\w+)\/(\w+)"": ""(\w+)"" = ""(\w+)""")]
        public void GivenITheFollowingSetting(string envName, string appName, string settingKey, string settingValue)
        {
            var url = string.Format("/environments/{0}/applications/{1}/settings", envName, appName);
            Result = Browser.Post(url, with =>
            {
                JsonClient(with);
                with.FormValue("key", settingKey);
                with.FormValue("value", settingValue);
            });
        }

        [Given(@"the following setting exist in ""(\w+)\/(\w+)"": ""(\w+)"" = ""(\w+)""")]
        public void GivenTheFollowingSettingExistIn(string envName, string appName, string settingKey, string settingValue)
        {
            GivenITheFollowingSetting(envName, appName, settingKey, settingValue);
        }

        [When(@"I get available settings for the application ""(\w+)"" in the ""(\w+)"" enviroment")]
        public void WhenIGetAvailableSettingsForTheApplicationInTheEnviroment(string appName, string envName)
        {
            var url = string.Format("/environments/{0}/applications/{1}/settings", envName, appName);
            Result = Browser.Get(url, JsonClient);
        }

        [Then(@"the setting ""(\w+)"" should be persisted in ""(\w+)\/(\w+)"" with value ""(\w+)""")]
        public void ThenTheSettingShouldBePersistedInWithValue(string settingKey, string envName, string appName, string settingValue)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var setting = session.Load<Environment>("environments/" + envName)
                    .GetApplication(appName)
                    .GetSetting(settingKey);

                Assert.That(setting.Value, Is.EqualTo(settingValue));
            }
        }

        [Then(@"I should the following settings:")]
        public void ThenIShouldTheFollowingSettings(Table settingsTable)
        {
            var settingsDictionary = settingsTable.CreateSet<Setting>().ToDictionary(s => s.Key, s => s.Value);
            var settingsFromResult = Deserialize<Dictionary<string,string>>(Result);

            CollectionAssert.AreEquivalent(settingsDictionary, settingsFromResult);
        }


        // Setup / boilerplate

        public SettingSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}