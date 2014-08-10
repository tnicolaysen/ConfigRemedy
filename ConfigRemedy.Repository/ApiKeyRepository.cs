using System.Linq;
using ConfigRemedy.Domain;
using Raven.Client;

namespace ConfigRemedy.Repository
{
    public interface IApiKeyRepository
    {
        ApiKey GetApiKyByHashedValue(string hashedValue);
        void Store(ApiKey apiKey);
    }

    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly IDocumentSession _session;

        public ApiKeyRepository(IDocumentSession session)
        {
            _session = session;
        }

        public ApiKey GetApiKyByHashedValue(string hashedValue)
        {
            var apiKey = _session
                .Query<ApiKey>()
                .FirstOrDefault(x => x.HashedValue == hashedValue);
            
            return apiKey;
        }

        public void Store(ApiKey apiKey)
        {
            _session.Store(apiKey);
            _session.SaveChanges();
        }
    }
}