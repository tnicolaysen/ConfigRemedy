using ConfigRemedy.Core;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using ConfigRemedy.Security.Annotations;
using Raven.Client;
using Serilog;

namespace ConfigRemedy.Security
{
    [UsedImplicitly]
    public sealed class AdminAccountRestorer
    {
        private readonly IDocumentStore _documentStore;
        private readonly IPasswordHasher _passwordHasher;

        public AdminAccountRestorer(IDocumentStore documentStore, IPasswordHasher passwordHasher)
        {
            _documentStore = documentStore;
            _passwordHasher = passwordHasher;
        }

        public void RestoreDefaultAdminAccount()
        {
            using (var session = _documentStore.OpenSession())
            {
                var userRepository = new UserRepository(session);
                var admin = userRepository.GetUserByUsername(Constants.DefaultAdminUsername);

                if (admin != null)
                    return;
               
                Log.Information("Restoring default admin account");

                var user = new User
                {
                    DisplayName = Constants.DefaultAdminDisplayName,
                    HashedPassword = _passwordHasher.CreateHash(Constants.DefaultAdminPassword),
                    Username = Constants.DefaultAdminUsername,
                };

                userRepository.Store(user);
            }
        }
    }
}