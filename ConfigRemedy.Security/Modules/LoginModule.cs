using System.Collections.Generic;
using ConfigRemedy.Core.Modules;
using Nancy;
using Nancy.Authentication.Token;
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
                var userName = (string)Request.Form.UserName;
                var password = (string)Request.Form.Password;

                var userIdentity = ValidateUser(userName, password);

                if (userIdentity == null)
                {
                    return HttpStatusCode.Unauthorized;
                }

                var token = tokenizer.Tokenize(userIdentity, Context);

                return new
                {
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

        private IUserIdentity ValidateUser(string userName, string password)
        {
            return new ConfiguratronUserIdentity
            {
                UserName = "JamesBond",
                Claims = new[] { "admin", "user"},
            };
        }
    }


    public class ConfiguratronUserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}