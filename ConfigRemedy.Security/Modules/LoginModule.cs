using ConfigRemedy.Core.Modules;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
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

                var user = ValidateUser(credentialns);

                if (user == null)
                {
                    return HttpStatusCode.Unauthorized;
                }

                var userIdentity = user.ToConfiguratronUserIdentity();

                var token = tokenizer.Tokenize(userIdentity, Context);

                return new
                {
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

        private User ValidateUser(Credentials credentials)
        {
            var user = _userRepository.GetUserByUsername(credentials.Username);
            if (user == null) return null;
            var hashedPassword = _hashedValueProvider.GetHash(credentials.Password);
            if (user.HashedPassword != hashedPassword) return null;

            return user;
        }
    }
}