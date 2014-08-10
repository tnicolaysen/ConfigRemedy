using System;
using ConfigRemedy.AcceptanceTests.Annotations;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using ConfigRemedy.Security;
using NUnit.Framework;
using System.Linq;
using TechTalk.SpecFlow;

namespace ConfigRemedy.AcceptanceTests.Steps
{
    [UsedImplicitly, MeansImplicitUse]
    [Binding]
    public class AuthenticationSteps : ModuleStepsBase
    {
        private ApiKeyRepository _apiRepository;
        private HashedValueProvider _hashedValueProvider;


        [Given(@"an ApiKey ""(.*)"" for user ""(.*)"" exist")]
        public void given_an_apikey_for_user_exists(string apikey, string userId)
        {
            var apiKey = new ApiKey
            {
                Created = DateTime.UtcNow,
                HashedValue = _hashedValueProvider.GetHash(apikey),
                Usage = "",
                UserId = userId
            };
            _apiRepository.Store(apiKey);
        }

        public AuthenticationSteps(DatabaseContext dbContext)
            : base(dbContext)
        {
            _apiRepository = new ApiKeyRepository(dbContext.EmbeddedStore.OpenSession());
            _hashedValueProvider = new HashedValueProvider();
        }
    }
}