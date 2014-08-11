using ConfigRemedy.Core.Modules;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using ConfigRemedy.Security.Nancy;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using Nancy.Security;

namespace ConfigRemedy.Security.Modules
{
    public class LoginModule : BaseModule
    {
        private readonly IHashedValueProvider _hashedValueProvider;
        private readonly IUserRepository _userRepository;
        public LoginModule(ITokenizer tokenizer, IHashedValueProvider hashedValueProvider, IUserRepository userRepository)            
        {
            _hashedValueProvider = hashedValueProvider;
            _userRepository = userRepository;
            Post["login"] = x =>
            {
                var credentialns = this.Bind<Credentials>();

                var userIdentity = ValidateUser(credentialns);

                if (userIdentity == null)
                {
                    return HttpStatusCode.Unauthorized;
                }

                var token = tokenizer.Tokenize(userIdentity, Context);

                return new
                {
                    UserId = userIdentity.UserId,
                    UserName = userIdentity.UserName,
                    DisplayName = userIdentity.DisplayName,
                    Role = userIdentity.Role,
                    Token = token,
                };
            };

            Get["login"] = _ =>
            {
                this.RequiresAuthentication();
                return "You are authenticated!";
            };

            Get["admin"] = _ =>
            {
                this.RequiresClaims(new[] { "admin" });
                return "You are authorized!";
            };
        }

        private ConfiguratronUserIdentity ValidateUser(Credentials credentials)
        {
            var user = _userRepository.GetUserByUsername(credentials.Username);
            if (user == null) return null;
            var hashedPassword = _hashedValueProvider.GetHash(credentials.Password);
            if (user.HashedPassword != hashedPassword) return null;

            return new ConfiguratronUserIdentity
            {
                UserName = user.Username,
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Role = "admin",
                Claims = new[] { "admin", "user"},
            };
        }
    }
}