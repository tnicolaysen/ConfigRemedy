using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Domain;
using NUnit.Framework;
using System.Linq;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class UsersSteps : ModuleStepsBase
    {
        private const string UsersBaseUrl = "api/users/";

        [When(@"I get available users")]
        public void WhenIGetAvailableUsers()
        {

            Result = Browser.Get(UsersBaseUrl, AuthenticatedJsonClient);
        }

        [When(@"I GET an user named ""(\w+)""")]
        public void WhenIGetAnUserNamed(string userName)
        {
            Result = Browser.Get(UsersBaseUrl + userName, AuthenticatedJsonClient);
        }
        
        [Then(@"there should be (\d+) users")]
        public void ThenThereShouldBeUsers(int numberOfUsers)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session.Query<User>().ToList();

                Assert.That(result.Count, Is.EqualTo(numberOfUsers));
            }
        }

        [Given(@"an user named ""(.*)"" exist")]
        public void GivenAnUserNamedExist(string userName)
        {
            When(string.Format(@"I POST a user with username ""{0}"" and display name ""displayName"" and email ""email"" and password ""password""", userName));
        }

        [Given(@"an user with name  ""(.*)"" and password ""(.*)"" exists")]
        public void GivenAnUserNamedExist(string userName, string password)
        {
            When(string.Format(@"I POST a user with username ""{0}"" and display name ""displayName"" and email ""email"" and password ""{1}""",
                userName, password));
        }


        [When(@"I POST a user with username ""(.*)"" and display name ""(.*)"" and email ""(.*)"" and password ""(.*)""")]
        public void when_I_post_a_user(string userName, string displayName, string email, string password)
        {
            Result = Browser.Post(UsersBaseUrl, with =>
            {
                AuthenticatedJsonClient(with);
                with.FormValue("userName", userName);
                with.FormValue("displayName", displayName);
                with.FormValue("email", email);
                with.FormValue("password", password);
            });
        }

        [Then(@"an user named ""(\w+)"" should be persisted")]
        public void ThenAnUserNamedShouldBePersisted(string userName)
        {
            using (var session = DbContext.EmbeddedStore.OpenSession())
            {
                var result = session
                    .Query<User>()
                    .Where(user => user.Username == userName)
                    .ToList();

                Assert.That(result, Is.Not.Empty, "Did not find the specified user");
            }
        }

        [Then(@"I should get an user model with name ""(\w+)""")]
        public void ThenIShouldGetAnUserModelWithName(string userName)
        {
            var user = Deserialize<User>(Result);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Username, Is.EqualTo(userName));
        }
        
        // Setup / boilerplate
        public UsersSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}