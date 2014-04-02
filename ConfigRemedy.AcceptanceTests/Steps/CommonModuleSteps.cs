using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [Binding]
    public class CommonModuleSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"the database is empty")]
        public void GivenTheDatabaseIsEmpty()
        {
            // TODO: Ensure?
        }

    }
}
