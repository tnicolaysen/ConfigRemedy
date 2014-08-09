using System.Collections.Generic;
using ConfigRemedy.Core.Modules;
using ConfigRemedy.Security.Domain;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.ModelBinding;
using Nancy.Security;
using Raven.Client;

namespace ConfigRemedy.Security.Modules
{
    public class LoginModule : BaseModule
    {
        private readonly IDocumentSession _session;
        private readonly IHashedPasswordProvider _hashedPasswordProvider;

        public LoginModule(ITokenizer tokenizer, IDocumentSession session, IHashedPasswordProvider hashedPasswordProvider)            
        {
            _session = session;
            _hashedPasswordProvider = hashedPasswordProvider;
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

        private User GetUser(string username)
        {
            return _session.Load<User>("users/" + username);
        }

        private ConfiguratronUserIdentity ValidateUser(Credentials credentials)
        {
            var user = GetUser(credentials.Username);
            if (user == null) return null;
            var hashedPassword = _hashedPasswordProvider.GenerateHashedPassword(credentials.Password);
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


    public class ConfiguratronUserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}