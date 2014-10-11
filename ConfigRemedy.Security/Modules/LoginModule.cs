using ConfigRemedy.Core.Modules;
using ConfigRemedy.Domain;
using ConfigRemedy.Repository;
using ConfigRemedy.Security.Annotations;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using Nancy.Security;

namespace ConfigRemedy.Security.Modules
{
    [UsedImplicitly]
    public class LoginModule : BaseModule
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public LoginModule(ITokenizer tokenizer, IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;

            Post["login"] = _ => PostLogin(tokenizer);
            Get["login"] = _ => GetLogin();
            Get["admin"] = _ => GetAdmin();
        }

        private dynamic GetAdmin()
        {
            this.RequiresClaims(new[] {"admin"});
            return "You are authorized!";
        }

        private dynamic GetLogin()
        {
            this.RequiresAuthentication();
            return "You are authenticated!";
        }

        private dynamic PostLogin(ITokenizer tokenizer)
        {
            var credentials = this.Bind<Credentials>();

            var user = ValidateUser(credentials);

            if (user == null)
                return HttpStatusCode.Unauthorized;

            var userIdentity = user.ToConfiguratronUserIdentity();

            var token = tokenizer.Tokenize(userIdentity, Context);

            return new
            {
                UserName = userIdentity.UserName,
                DisplayName = userIdentity.DisplayName,
                Role = userIdentity.Role,
                Token = token,
            };
        }

        private User ValidateUser(Credentials credentials)
        {
            var user = _userRepository.GetUserByUsername(credentials.Username);
            if (user == null) 
                return null;
            
            if (!_passwordHasher.ValidatePassword(credentials.Password, user.HashedPassword)) 
                return null;

            return user;
        }
    }
}