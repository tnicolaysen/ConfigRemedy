using Nancy.Testing;
using Raven.Imports.Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class ModuleStepsBase : TechTalk.SpecFlow.Steps
    {
        protected DatabaseContext DbContext
        {
            get { return ScenarioContext.Current.Get<DatabaseContext>("DatabaseContext"); }
            set { ScenarioContext.Current.Set(value, "DatabaseContext"); }
        }

        protected Browser Browser
        {
            get { return ScenarioContext.Current.Get<Browser>("Browser"); }
            set { ScenarioContext.Current.Set(value, "Browser"); }
        }

        protected BrowserResponse Result
        {
            get { return ScenarioContext.Current.Get<BrowserResponse>("Result"); }
            set { ScenarioContext.Current.Set(value, "Result"); }
        }

        protected ModuleStepsBase(DatabaseContext dbContext = null)
        {
            DbContext = dbContext;
        }

        protected void JsonClient(BrowserContext with)
        {
            with.Header("accept", "application/json");
            with.HttpRequest();
        }

        protected T Deserialize<T>(BrowserResponse response)
        {
            var jsonString = response.Body.AsString();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}