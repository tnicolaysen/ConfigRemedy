using Nancy.Testing;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class ModuleStepsBase : TechTalk.SpecFlow.Steps
    {
        public DatabaseContext DbContext
        {
            get { return ScenarioContext.Current.Get<DatabaseContext>("DatabaseContext"); }
            set { ScenarioContext.Current.Set(value, "DatabaseContext"); }
        }

        public Browser Browser
        {
            get { return ScenarioContext.Current.Get<Browser>("Browser"); }
            set { ScenarioContext.Current.Set(value, "Browser"); }
        }

        public BrowserResponse Result
        {
            get { return ScenarioContext.Current.Get<BrowserResponse>("Result"); }
            set { ScenarioContext.Current.Set(value, "Result"); }
        }

        protected ModuleStepsBase(DatabaseContext dbContext)
        {
            DbContext = dbContext;
        }

        protected void JsonClient(BrowserContext with)
        {
            with.Header("accept", "application/json");
            with.HttpRequest();
        }
    }
}