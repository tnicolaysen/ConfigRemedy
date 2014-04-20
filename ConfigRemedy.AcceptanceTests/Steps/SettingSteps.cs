using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class SettingSteps : ModuleStepsBase
    {
        [Given(@"the following setting exist in ""(\w+)\/(\w+)"": ""(\w+)"" = ""(.+)""")]
        public void GivenTheFollowingSettingExistIn(string envName, string appName, string settingKey, string settingValue)
        {
            GivenITheFollowingSetting(envName, appName, settingKey, settingValue);
        }

        [When(@"I get available settings for the application ""(\w+)"" in the ""(\w+)"" enviroment")]
        public void WhenIGetAvailableSettingsForTheApplicationInTheEnviroment(string appName, string envName)
        {
            var url = string.Format("/environments/{0}/{1}/settings", envName, appName);
            Result = Browser.Get(url, JsonClient);
        }

        [When(@"I POST the following setting to ""(\w+)\/(\w+)"": ""(\w+)"" = ""(.+)""")]
        public void GivenITheFollowingSetting(string envName, string appName, string settingKey, string settingValue)
        {
            var url = string.Format("/environments/{0}/{1}/settings", envName, appName);
            Result = Browser.Post(url, with =>
            {
                JsonClient(with);
                with.FormValue("key", settingKey);
                with.FormValue("value", settingValue);
            });
        }

        [When(@"I get the setting ""(\w+)"" in ""(\w+)\/(\w+)""")]
        public void WhenIGetTheSettingFor(string settingKey, string envName, string appName)
        {
            var url = string.Format("/environments/{0}/{1}/{2}", envName, appName, settingKey);
            Result = Browser.Get(url, JsonClient);
        }

        [Then(@"I should get a string identical to ""(.*)""")]
        public void ThenIShouldGetAStringIdenticalTo(string settingValue)
        {
            Assert.That(Result.Body.AsString(), Is.EqualTo(settingValue));
        }


        [Then(@"the setting ""(\w+)"" should be persisted in ""(\w+)\/(\w+)"" with value ""(.+)""")]
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