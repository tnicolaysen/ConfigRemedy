﻿using ConfigRemedy.Domain;
using Nancy.Testing;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class SettingSteps : ModuleStepsBase
    {
        [Given(@"the following override exist in ""(\w+)\/(\w+)"": ""(\w+)"" = ""(.+)""")]
        public void GivenTheFollowingOverrideExistIn(string envName, string appName, string settingKey, string settingValue)
        {
            GivenITheFollowingSettingOverride(envName, appName, settingKey, settingValue);
        }

        [Given(@"that ""(\w+)"" have the following settings:")]
        public void GivenThatHaveTheFollowingSettings(string appName, Table settingsTable)
        {   
            var settings = settingsTable.CreateSet<Setting>();
            foreach (var setting in settings)
            {
                Result = Browser.Post(string.Format("/applications/{0}/settings", appName), context =>
                {
                    JsonClient(context);
                    context.JsonBody(setting);
                });
            }
        }

        [When(@"I get available settings for the application ""(\w+)"" in the ""(\w+)"" enviroment")]
        public void WhenIGetAvailableSettingsForTheApplicationInTheEnviroment(string appName, string envName)
        {
            var url = string.Format("/environments/{0}/{1}/settings", envName, appName);
            Result = Browser.Get(url, JsonClient);
        }

        [When(@"I get available settings for the application ""(\w+)""")]
        public void WhenIGetAvailableSettingsForTheApplication(string appName)
        {
            var url = string.Format("/applications/{0}", appName);
            Result = Browser.Get(url, JsonClient);
        }

        [When(@"I POST the following settings to ""(.*)"":")]
        public void WhenIPOSTTheFollowingSettingsTo(string appName, Table settingsTable)
        {
            GivenThatHaveTheFollowingSettings(appName, settingsTable);;
        }

        [When(@"I POST the following setting override to ""(\w+)\/(\w+)"": ""(.+)"" = ""(.+)""")]
        public void GivenITheFollowingSettingOverride(string appName, string envName, string settingKey, string settingValue)
        {
            var url = string.Format("/applications/{0}/settings/{1}", appName, envName);
            Result = Browser.Post(url, with =>
            {
                JsonClient(with);
                with.FormValue("key", settingKey);
                with.FormValue("value", settingValue);
            });
        }

        [Given(@"the following overrides exist:")]
        public void GivenTheFollowingOverridesExist(Table table)
        {
            foreach (var row in table.Rows)
            {
                var app = row["App"];
                var env = row["Environment"];
                var key = row["Key"];
                var value = row["Value"];

                GivenITheFollowingSettingOverride(app, env, key, value);
            }
        }


        [When(@"I get the setting ""(\w+)"" in ""(\w+)\/(\w+)""")]
        public void WhenIGetTheSettingFor(string settingKey, string appName, string envName)
        {
            var url = string.Format("/applications/{0}/settings/{1}/{2}", appName, envName, settingKey);
            Result = Browser.Get(url, JsonClient);
        }

        [Then(@"I should get an empty settings list")]
        public void ThenIShouldGetAnEmptySettingsList()
        {
            var appFromResult = Deserialize<Application>(Result);
            Assert.That(appFromResult.Settings, Is.Empty);
        }


        [Then(@"I should get a string identical to ""(.*)""")]
        public void ThenIShouldGetAStringIdenticalTo(string settingValue)
        {
            Assert.That(Result.Body.AsString(), Is.EqualTo(settingValue));
        }

        [Then(@"the setting ""(.*)"" should be persisted in ""(.*)"" with default value ""(.*)""")]
        public void ThenTheSettingShouldBePersistedInWithDefaultValue(string settingKey, string appName, string settingValue)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var setting = session.Load<Application>("applications/" + appName)
                    .GetSetting(settingKey);

                Assert.That(setting.DefaultValue, Is.EqualTo(settingValue));
            }
        }

        [Then(@"I should the following settings:")]
        public void ThenIShouldTheFollowingSettings(Table settingsTable)
        {
            var expectedSettings = settingsTable.CreateSet<Setting>();
            var actualSettings = Deserialize<Application>(Result).Settings;

            CollectionAssert.AreEquivalent(expectedSettings, actualSettings);
        }


        // Setup / boilerplate

        public SettingSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}