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

        public LoginModule(ITokenizer tokenizer, IDocumentSession session)            
        {
            _session = session;
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
            return new ConfiguratronUserIdentity
            {
                UserName = "JamesBond",
                UserId = "user/1",
                Role = "admin",
                Claims = new[] { "admin", "user"},
            };
        }
    }


    public class ConfiguratronUserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}