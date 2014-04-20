﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34011
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ConfigRemedy.AcceptanceTests.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Setting")]
    public partial class SettingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Setting.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Setting", "In order manage settings for a given application in a given environment\r\nAs a pro" +
                    "grammer\r\nI want to have a REST API to manage them", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
 testRunner.Given("I have a JSON client", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Getting settings for an app. without settings")]
        public virtual void GettingSettingsForAnApp_WithoutSettings()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting settings for an app. without settings", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 10
 testRunner.Given("an environment named \"test\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.Given("\"test\" has the application \"zaphod\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
 testRunner.When("I get available settings for the application \"zaphod\" in the \"test\" enviroment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.Then("I should get HTTP OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
 testRunner.And("I should get an empty object", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Getting settings for an app.")]
        public virtual void GettingSettingsForAnApp_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting settings for an app.", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 17
 testRunner.Given("an environment named \"test\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.Given("\"test\" has the application \"scroogle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.And("the following setting exist in \"test/scroogle\": \"setting1\" = \"a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.And("the following setting exist in \"test/scroogle\": \"setting2\" = \"b\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.When("I get available settings for the application \"scroogle\" in the \"test\" enviroment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
 testRunner.Then("I should get HTTP OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Key",
                        "Value"});
            table1.AddRow(new string[] {
                        "setting1",
                        "a"});
            table1.AddRow(new string[] {
                        "setting2",
                        "b"});
#line 23
 testRunner.And("I should the following settings:", ((string)(null)), table1, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get specific setting value")]
        public virtual void GetSpecificSettingValue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get specific setting value", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 29
 testRunner.Given("an environment named \"prod\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.Given("\"prod\" has the application \"fooble\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.And("the following setting exist in \"prod/fooble\": \"version\" = \"1.0 RC1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.When("I get the setting \"version\" in \"prod/fooble\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
 testRunner.Then("I should get HTTP OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.And("I should get a string identical to \"1.0 RC1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get specific setting that don\'t exist")]
        public virtual void GetSpecificSettingThatDonTExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get specific setting that don\'t exist", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 37
 testRunner.Given("an environment named \"prod\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.Given("\"prod\" has the application \"fooble\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.When("I get the setting \"idontexist\" in \"prod/fooble\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
 testRunner.Then("I should get HTTP NotFound", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Adding settings to an application")]
        public virtual void AddingSettingsToAnApplication()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Adding settings to an application", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 43
 testRunner.Given("an environment named \"test\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
 testRunner.Given("\"test\" has the application \"scroogle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 45
 testRunner.When("I POST the following setting to \"test/scroogle\": \"retries\" = \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
 testRunner.Then("I should get HTTP Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.And("the setting \"retries\" should be persisted in \"test/scroogle\" with value \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.And("location header should contain url for \"environments/test/scroogle/retries\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Adding duplicate setting is not allowed")]
        public virtual void AddingDuplicateSettingIsNotAllowed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Adding duplicate setting is not allowed", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 51
 testRunner.Given("an environment named \"test\" exist", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 52
 testRunner.Given("\"test\" has the application \"scroogle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 53
 testRunner.And("the following setting exist in \"test/scroogle\": \"version\" = \"1.0 RC1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.When("I POST the following setting to \"test/scroogle\": \"version\" = \"something else\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.Then("I should get HTTP Forbidden with reason \"Duplicates are not allowed\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion