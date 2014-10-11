using System;
using Nancy.Authentication.Token;

namespace ConfigRemedy.Security.Nancy
{
    public class ConfiguratronAuthenticationConfiguration
    {
        private readonly IUserResolver _userResolver;

        public ConfiguratronAuthenticationConfiguration(ITokenizer tokenizer, IUserResolver userResolver)
        {
            if (tokenizer == null)
                throw new ArgumentNullException("tokenizer");

            Tokenizer = tokenizer;
            _userResolver = userResolver;
        }

        public ITokenizer Tokenizer { get; private set; }


        public IUserResolver UserResolver
        {
            get { return _userResolver; }
        }
    }
}