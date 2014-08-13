using ConfigRemedy.Repository;
using ConfigRemedy.Security.Modules;
using Nancy.Security;

namespace ConfigRemedy.Security.Nancy
{
    public interface IUserResolver
    {
        IUserIdentity GetUser(string apiKey);
    }

    public class UserResolver : IUserResolver
    {
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashedValueProvider _hashedValueProvider;

        public UserResolver(IApiKeyRepository apiKeyRepository, IUserRepository userRepository, IHashedValueProvider hashedValueProvider)
        {
            _apiKeyRepository = apiKeyRepository;
            _userRepository = userRepository;
            _hashedValueProvider = hashedValueProvider;
        }

        public IUserIdentity GetUser(string apiKey)
        {
            var hashedApiKey = _hashedValueProvider.GetHash(apiKey);
            var key = _apiKeyRepository.GetApiKyByHashedValue(hashedApiKey);
            if (key == null) return null;

            var user = _userRepository.GetUserById(key.UserId);
            if (user == null) return null;

            return new ConfiguratronUserIdentity
            {
                UserName = user.Username,
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Role = "admin",
                Claims = new[] { "admin", "user" },   
            };
        }
    }
}