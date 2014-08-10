using ConfigRemedy.Core;
using ConfigRemedy.Domain;
using Raven.Client;

namespace ConfigRemedy.Security
{
    public interface IAdminAccountRestorer
    {
        void RestoreDefaultAdminAccount();
    }

    public class AdminAccountRestorer : IAdminAccountRestorer
    {
        private readonly IDocumentStore _documentStore;
        private readonly IHashedValueProvider _hashedValueProvider;
        public AdminAccountRestorer(IDocumentStore documentStore, IHashedValueProvider hashedValueProvider)
        {
            _documentStore = documentStore;
            _hashedValueProvider = hashedValueProvider;
        }

        public void RestoreDefaultAdminAccount()
        {
            using (var session = _documentStore.OpenSession())
            {
                var admin = session.Load<User>("users/" + Constants.DefaultAdminUsername);
                if (admin != null) return;

                var user = new User
                {
                    DisplayName = Constants.DefaultAdminDisplayName,
                    HashedPassword = _hashedValueProvider.GetHash(Constants.DefaultAdminPassword),
                    Username = Constants.DefaultAdminUsername,
                };

                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}